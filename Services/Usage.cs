using System;
using ServerTemperatureSystem.Models;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ServerTemperatureSystem.Services
{
    public class Usage
    {
        public float CPUUsage { get; set; }
        private List<long> cpuActivePrevs = new List<long>();
        private List<long> cpuTotalPrevs = new List<long>();
        private List<long> cpuActiveCurr = new List<long>();
        private List<long> cpuTotalCurr = new List<long>();
        private List<CPUParams> CpuParamsTemp = new List<CPUParams>();
        public void SetUsage(ref Components components)
        {
            CountCPUFirstProbe();
            CountCPUSecondProbe();
            CountCPUUsage(components);
            SetMemoryUsage(components);
        }
        public void GetCPUParams()
        {
            CpuParamsTemp = new List<CPUParams>();
            string path = "/proc/stat";
            FileStream stream = File.OpenRead(path);
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                string currLine = sr.ReadLine();
                if (currLine.Contains("cpu"))
                    SetCPUParams(currLine);
            }
            stream.Close();
        }
        private void SetCPUParams(string cpuParams)
        {
            string pattern = @"\s+";
            var splittedParams = Regex.Split(cpuParams, pattern)
            .Where(p => p != string.Empty)
            .ToList();

            CpuParamsTemp.Add(new CPUParams(splittedParams));
        }
        private void CountCPUFirstProbe()
        {
            GetCPUParams();

            foreach (var p in CpuParamsTemp)
            {
                cpuActivePrevs.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal);
                cpuTotalPrevs.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal + p.Idle + p.Iowait);
            }
            Thread.Sleep(5000);
        }
        private void CountCPUSecondProbe()
        {
            GetCPUParams();

            foreach (var p in CpuParamsTemp)
            {
                cpuActiveCurr.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal);
                cpuTotalCurr.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal + p.Idle + p.Iowait);
            }
        }
        private void CountCPUUsage(Components components)
        {
            for (int i = 0; i <= components.CPU.Cores.Count; i++)
            {
                if (i == 0)
                    components.CPU.Usage = 100 * (cpuActiveCurr[i] - cpuActivePrevs[i]) / (cpuTotalCurr[i] - cpuTotalPrevs[i]);
                else
                {
                    components.CPU.Cores[i - 1].Usage = 100 * (cpuActiveCurr[i] - cpuActivePrevs[i]) / (cpuTotalCurr[i] - cpuTotalPrevs[i]);
                    components.CPU.Cores[i - 1].CoreName = "Core" + (i - 1).ToString();
                }
            }
        }
        private void SetMemoryUsage(Components c)
        {
            string memoryDetails = "";
            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \"free -m\"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                string current = proc.StandardOutput.ReadLine();
                if (current.Contains("Mem:"))
                    memoryDetails = current;
            }

            c.Memory = SetMemoryParams(memoryDetails);
            c.Memory.Usage = 100*(c.Memory.Total - c.Memory.Free)/c.Memory.Total;           
        }
        private Memory SetMemoryParams(string memDetails)
        {
            string pattern = @"\d+";
            Regex rgx = new Regex(pattern);
            MatchCollection temp = rgx.Matches(memDetails);

            return new Memory
            {
                Total = int.Parse(temp[0].Value),
                Used = int.Parse(temp[1].Value),
                Free = int.Parse(temp[2].Value),
                Shared = int.Parse(temp[3].Value),
                BufforCache = int.Parse(temp[4].Value)
            };
        }
    }
}