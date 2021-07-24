using System;
using System.Collections.Generic;
using System.Threading;
using ServerTemperatureSystem.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerTemperatureSystem.Models
{
    public class Components
    {
        public CPU CPU { get; set; }
        public Motherboard MB { get; set; }
        public Memory Memory { get; set; }
    }
    public abstract class TempParameters
    {
        public double Temp { get; set; }
        public double CriticalTemp { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
    }
    public class CPU : TempParameters
    {
        public string Name { get; set; }
        public float Usage { get; set; }
        public List<Core> Cores { get; set; }
    }
    public class Core : TempParameters
    {
        public float Usage { get; set; }
        public string CoreName { get; set; }
    }
    public class CPUParams
    {
        public CPUParams(List<string> parameters)
        {
            Name = parameters[0];
            User = long.Parse(parameters[1]);
            Nice = long.Parse(parameters[2]);
            System = long.Parse(parameters[3]);
            Idle = long.Parse(parameters[4]);
            Iowait = long.Parse(parameters[5]);
            Irq = long.Parse(parameters[6]);
            Softirq = long.Parse(parameters[7]);
            Steal = long.Parse(parameters[8]);
            Guest = long.Parse(parameters[9]);
            Guest_nice = long.Parse(parameters[10]);
        }
        public string Name { get; set; }
        public long User { get; set; }
        public long Nice { get; set; }
        public long System { get; set; }
        public long Idle { get; set; }
        public long Iowait { get; set; }
        public long Irq { get; set; }
        public long Softirq { get; set; }
        public long Steal { get; set; }
        public long Guest { get; set; }
        public long Guest_nice { get; set; }
    }
    public class Memory
    {
        public int Usage { get; set; }
        public int Total { get; set; }
        public int Used { get; set; }
        public int Free { get; set; }
        public int Shared { get; set; }
        public int BufforCache { get; set; }

    }
    public class Motherboard : TempParameters { }

    public class ComponentReadings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        //public Component Details { get; set; }
        public int Usage { get; set; }
        public float Temperature { get; set; }
        public DateTime Date { get; set; }
    }
    public class Component
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public virtual IList<ComponentReadings> Readings { get; set; }
    }
}