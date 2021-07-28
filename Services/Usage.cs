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
        private List<int> cpuActivePrevs = new List<int>();
        private List<int> cpuTotalPrevs = new List<int>();
        private List<int> cpuActiveCurr = new List<int>();
        private List<int> cpuTotalCurr = new List<int>();
        private List<CPUParams> CpuParamsTemp = new List<CPUParams>();
        private int usedMemory = 0;
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
            int i = 0;
            components.CPU.UsageReadings = new List<UsageDetails>{
                        new UsageDetails{
                            Usage = 100 * (cpuActiveCurr[0] - cpuActivePrevs[0]) / (cpuTotalCurr[0] - cpuTotalPrevs[0]),
                            Date = DateTime.Now
                        }
                    };

            foreach (var item in components.CPU.Cores)
            {
                item.UsageReadings = new List<UsageDetails>{
                        new UsageDetails{
                            Usage = 100 * (cpuActiveCurr[i] - cpuActivePrevs[i]) / (cpuTotalCurr[i] - cpuTotalPrevs[i]),
                            Date = DateTime.Now
                        }
                    };
                i++;
            }
        }
        private void SetMemoryUsage(Components c)
        {
            c.Memory  = SetMemoryParams();           
            c.Memory.UsageReadings = new List<UsageDetails>{
                new UsageDetails{
                    Usage = 100*usedMemory/c.Memory.Total,
                    Date = DateTime.Now
                }
            };
        }
        private string GetMemoryDetails()
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

            return memoryDetails;
        }
        public Memory SetMemoryParams()
        {
            string memDetails = GetMemoryDetails();
            string pattern = @"\d+";
            Regex rgx = new Regex(pattern);
            MatchCollection temp = rgx.Matches(memDetails);

            usedMemory = int.Parse(temp[1].Value);
            return new Memory
            {
                Total = int.Parse(temp[0].Value),
                Name = "Memory"
            };
        }
    }
}