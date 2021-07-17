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
            CountFirstProbe();
            CountSecondProbe();
            CountUsage();
        }
        public void GetCPUParams()
        {
            string path = "/proc/stat";
            FileStream stream = File.OpenRead(path);
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                string currLine = sr.ReadLine();
                if (currLine.Contains("cpu"))
                    SetCPUParams(currLine);
            }
        }
        private void SetCPUParams(string cpuParams)
        {
            CpuParamsTemp = new List<CPUParams>();
            string pattern = @"\s+";
            var splittedParams = Regex.Split(cpuParams, pattern)
            .Where(p => p != string.Empty)
            .ToList();

            CpuParamsTemp.Add(new CPUParams(splittedParams));
        }
        private void CountFirstProbe()
        {
            GetCPUParams();

            foreach (var p in CpuParamsTemp)
            {
                cpuActivePrevs.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal);
                cpuTotalPrevs.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal + p.Idle + p.Iowait);
            }
        }
        private void CountSecondProbe()
        {
            GetCPUParams();

            foreach (var p in CpuParamsTemp)
            {
                cpuActiveCurr.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal);
                cpuTotalCurr.Add(p.User + p.System + p.Nice + p.Softirq + p.Steal + p.Idle + p.Iowait);
            }
        }
        private void CountUsage()
        {

        }
    }
}