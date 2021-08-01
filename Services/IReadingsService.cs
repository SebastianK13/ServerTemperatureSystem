using System;
using ServerTemperatureSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IReadingsService
{
    Task<Components> GetReadings(DateTime date);
    Task<List<bool>> ComponentsExisting(); 
    Task InsertCPU(CPU cpu);
    Task InsertMobo(Motherboard mobo);
    Task InsertMemory(Memory memory);
    Task InsertCurrentReadings(Components components);
    Task RemoveReadingsOlderThan24h();
    Task<double> GetTempMinValue(int id, string componentName);
    Task<double> GetTempMaxValue(int id, string componentName);
    Task<int> GetUsageMinValue(int id, string componentName);
    Task<int> GetUsageMaxValue(int id, string componentName);
    //TemperatureDetails GetTempDetails();
}