using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ServerTemperatureSystem.Models;

namespace ServerTemperatureSystem.Services
{
    public class Temperatures
    {
        public Components GetSystemTemps()
        {

            List<string> monitRecords = new List<string>();
            Process proc = new Process();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = $"-c 'sensors'";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
                monitRecords.Add(proc.StandardOutput.ReadLine());

            Components components = new Components();
            components.CPU = FindCPUDetails(monitRecords);
            components.MB = FindMBDetails(monitRecords);

            return components;
        }
        private Motherboard FindMBDetails(List<string> tempDetails)
        {
            Motherboard mb = new Motherboard();
            int numberOfSensors = 0;
            foreach (string c in tempDetails)
                if (c.ToLower().Contains("temp"))
                {
                    var currentLine = FindMatches(c);
                    if (currentLine.Count() == 2)
                    {
                        if (mb.CriticalTemp == 0)
                            mb.CriticalTemp = float.Parse(currentLine.Last().ToString());

                        numberOfSensors++;
/*                         mb.Temp += float.Parse(currentLine.First().ToString()); */
                    }
                }
            //mb.Temp = Math.Round(mb.Temp / numberOfSensors, 1);
            return mb;
        }
        private CPU FindCPUDetails(List<string> tempDetails)
        {
            CPU cpu = new CPU();
            List<Core> cores = new List<Core>();
            int coreNum = 0;
            foreach (string c in tempDetails)
            {
                var currentLine = FindMatches(c);
                if (currentLine.Count() >= 2)
                {
                    if (c.ToLower().Contains("core"))
                    {
                        float temp = float.Parse(currentLine.First().ToString());
                        cores.Add(new Core{
                            CriticalTemp = float.Parse(currentLine.Last().ToString()),
                            //Temp = temp,
                            Name = "Core"+coreNum
                        });
                        coreNum++;
                    }
                    else if (c.ToLower().Contains("package"))
                    {
                        cpu.CriticalTemp = float.Parse(currentLine.Last().ToString());
                        cpu.Name = "CPU";
                        //cpu.Temp = float.Parse(currentLine.First().ToString());
                    }
                }
            }
            cpu.Cores = cores;

            return cpu;
        }
        private MatchCollection FindMatches(string c)
        {
            string pattern = @"\d*\.\d*";
            Regex rgx = new Regex(pattern);
            return rgx.Matches(c);
        }
    }
}