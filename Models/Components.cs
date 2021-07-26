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
    public class UsageDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int Usage { get; set; }
        public DateTime Date { get; set; }
        public virtual CPU CPU { get; set; }
        public virtual Core Core { get; set; }
        public virtual Memory Memory { get; set; }
    }
    public class TemperatureDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public double Temperature { get; set; }
        public DateTime Date { get; set; }
        public virtual CPU CPU { get; set; }
        public virtual Core Core { get; set; }
        public virtual Motherboard Mobo { get; set; }
    }
    public class Core
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double CriticalTemp { get; set; }
        public virtual List<UsageDetails> UsageReadings { get; set; }
        public virtual List<TemperatureDetails> TemperatureReadings { get; set; }
        public virtual CPU CPU { get; set; }
    }
    public class CPU
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double CriticalTemp { get; set; }
        public virtual List<UsageDetails> UsageReadings { get; set; }
        public virtual List<TemperatureDetails> TemperatureReadings { get; set; }
        public virtual List<Core> Cores { get; set; }
    }
    public class Memory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public virtual List<UsageDetails> UsageReadings { get; set; }

    }
    public class Motherboard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double CriticalTemp { get; set; }
        public virtual List<TemperatureDetails> TemperatureReadings { get; set; }
    }
    public class CPUParams
    {
        public CPUParams(List<string> parameters)
        {
            Name = parameters[0];
            User = Int32.Parse(parameters[1]);
            Nice = Int32.Parse(parameters[2]);
            System = Int32.Parse(parameters[3]);
            Idle = Int32.Parse(parameters[4]);
            Iowait = Int32.Parse(parameters[5]);
            Irq = Int32.Parse(parameters[6]);
            Softirq = Int32.Parse(parameters[7]);
            Steal = Int32.Parse(parameters[8]);
            Guest = Int32.Parse(parameters[9]);
            Guest_nice = Int32.Parse(parameters[10]);
        }
        public string Name { get; set; }
        public int User { get; set; }
        public int Nice { get; set; }
        public int System { get; set; }
        public int Idle { get; set; }
        public int Iowait { get; set; }
        public int Irq { get; set; }
        public int Softirq { get; set; }
        public int Steal { get; set; }
        public int Guest { get; set; }
        public int Guest_nice { get; set; }
    }
    /*     public abstract class TempParameters
    {
        public double Temp { get; set; }
        public double CriticalTemp { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
    } */

    /*     public class ComponentReadings
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }
            public virtual Component Details { get; set; }
            public int Usage { get; set; }
            public float Temperature { get; set; }
            public DateTime Date { get; set; }
        } */
    /*     public class Component
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }
            public String Name { get; set; }
            public virtual List<ComponentReadings> Readings { get; set; }
            public virtual AdditionalInfofrmations AdditionalInfo { get; set; }
        }
        public class AdditionalInfofrmations
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }
            public double CriticalTemp { get; set; }
            public int Total { get; set; }
            public int Used { get; set; }
            public int Free { get; set; }
            public int Shared { get; set; }
            public int BufforCache { get; set; }
        }
        public class Core
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }
            public String Name { get; set; }
            public virtual List<ComponentReadings> Readings { get; set; }
            public virtual AdditionalInfofrmations AdditionalInfo { get; set; }
        } */
}