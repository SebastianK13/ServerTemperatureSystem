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
            var monitRecords = GetReadings();
            Components components = new Components();
            components.CPU = FindCPUDetails(monitRecords);
            components.MB = FindMBDetails(monitRecords);

            return components;
        }
        private List<string> GetReadings ()
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

            return monitRecords;
        }
        public Components GetCurrentTemps()
        {
            var readings = GetReadings();
            
            Components components = new Components();
            components.CPU = GetCPUTemps(readings);
            components.MB = GetMoboTemps(readings);

            return components;

        }
        private Motherboard GetMoboTemps(List<string> readings)
        {
            Motherboard mb = new Motherboard();
            int numberOfSensors = 0;
            float moboTemperature = 0;
            foreach (string c in readings)
                if (c.ToLower().Contains("temp"))
                {
                    var currentLine = FindMatches(c);
                    if (currentLine.Count() == 2)
                    {
                        numberOfSensors++;
                        moboTemperature += float.Parse(currentLine.First().ToString());
                    }
                }

            mb.TemperatureReadings = new List<TemperatureDetails>();
            mb.TemperatureReadings.Add(new TemperatureDetails{
                Temperature = Math.Round(moboTemperature / numberOfSensors, 1),
                Date = DateTime.Now
            });

            return mb;
        }
        private CPU GetCPUTemps(List<string> readings)
        {
            CPU cpu = new CPU();
            List<Core> cores = new List<Core>();
            foreach (string c in readings)
            {
                var currentLine = FindMatches(c);
                if (currentLine.Count() >= 2)
                {
                    if (c.ToLower().Contains("core"))
                    {
                        float temp = float.Parse(currentLine.First().ToString());
                        cores.Add(new Core{
                            TemperatureReadings = new List<TemperatureDetails>{
                                new TemperatureDetails{
                                    Temperature = temp,
                                    Date = DateTime.Now
                                }
                            }
                        });
                    }
                    else if (c.ToLower().Contains("package"))
                    {
                        cpu.TemperatureReadings = new List<TemperatureDetails>{
                            new TemperatureDetails{
                                Temperature = float.Parse(currentLine.First().ToString()),
                                Date = DateTime.Now
                            }
                        };
                    }
                }
            }
            cpu.Cores = cores;

            return cpu;
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
                    }
                }
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