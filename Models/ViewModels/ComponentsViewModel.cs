using System;
using System.Collections.Generic;
using ServerTemperatureSystem.Models;
using System.Linq;

public class ComponentsViewModel
{
    public ComponentsViewModel(Components components)
    {
        List<CoreViewModel> cores = new List<CoreViewModel>();
        foreach (var c in components.CPU.Cores)
        {
            cores.Add(new CoreViewModel
            {
                Name = c.Name,
                CriticalTemp = c.CriticalTemp,
                UsageReadings = ModelConverter.SetUsage(c.UsageReadings.ToList()),
                TemperatureReadings = ModelConverter
                    .SetTemperatures(c.TemperatureReadings.OrderBy(d=>d.Date).ToList())
            });
        };
        CPU = new CPUViewModel
        {
            Name = components.CPU.Name,
            CriticalTemp = components.CPU.CriticalTemp,
            UsageReadings = ModelConverter.SetUsage(components.CPU.UsageReadings.ToList()),
            TemperatureReadings = ModelConverter
                .SetTemperatures(components.CPU.TemperatureReadings.OrderBy(d=>d.Date).ToList()),
            Cores = cores,
            CurrentUsage = components.CPU.UsageReadings.OrderBy(d=>d.Date).Last().Usage
        };
        Mobo = new MotherboardViewModel
        {
            Name = components.MB.Name,
            CriticalTemp = components.MB.CriticalTemp,
            TemperatureReadings = ModelConverter
                .SetTemperatures(components.MB.TemperatureReadings.OrderBy(d=>d.Date).ToList())
        };
        Memory = new MemoryViewModel
        {
            Name = components.Memory.Name,
            Total = components.Memory.Total,
            UsageReadings = ModelConverter
                .SetUsage(components.Memory.UsageReadings.OrderBy(d=>d.Date).ToList()),
            CurrentUsage = components.Memory.UsageReadings.OrderBy(d=>d.Date).Last().Usage
        };
    }
    public CPUViewModel CPU { get; set; }
    public MotherboardViewModel Mobo { get; set; }
    public MemoryViewModel Memory { get; set; }
}

public class UsageDetailsViewModel
{
    public int Usage { get; set; }
    public DateTime Date { get; set; }
}
public class TemperatureDetailsViewModel
{
    public double Temperature { get; set; }
    public DateTime Date { get; set; }
}
public class CoreViewModel
{
    public string Name { get; set; }
    public double CriticalTemp { get; set; }
    public List<UsageDetailsViewModel> UsageReadings { get; set; }
    public List<TemperatureDetailsViewModel> TemperatureReadings { get; set; }
}
public class CPUViewModel
{
    public string Name { get; set; }
    public double CriticalTemp { get; set; }
    public int CurrentUsage { get; set; }
    public List<UsageDetailsViewModel> UsageReadings { get; set; }
    public List<TemperatureDetailsViewModel> TemperatureReadings { get; set; }
    public List<CoreViewModel> Cores { get; set; }
}
public class MemoryViewModel
{
    public string Name { get; set; }
    public int Total { get; set; }
    public int CurrentUsage { get; set; }
    public List<UsageDetailsViewModel> UsageReadings { get; set; }

}
public class MotherboardViewModel
{
    public string Name { get; set; }
    public double CriticalTemp { get; set; }
    public List<TemperatureDetailsViewModel> TemperatureReadings { get; set; }
}
public static class ModelConverter
{

    public static List<UsageDetailsViewModel> SetUsage(List<UsageDetails> details)
    {
        List<UsageDetailsViewModel> detailsVM = new List<UsageDetailsViewModel>();
        foreach (var d in details)
        {
            detailsVM.Add(new UsageDetailsViewModel
            {
                Usage = d.Usage,
                Date = d.Date
            });
        }

        return detailsVM;
    }
    public static List<TemperatureDetailsViewModel> SetTemperatures(List<TemperatureDetails> details)
    {
        List<TemperatureDetailsViewModel> detailsVM = new List<TemperatureDetailsViewModel>();
        foreach (var d in details)
        {
            detailsVM.Add(new TemperatureDetailsViewModel
            {
                Temperature = d.Temperature,
                Date = d.Date
            });
        }

        return detailsVM;
    }
}