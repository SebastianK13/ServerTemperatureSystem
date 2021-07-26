using System;
using ServerTemperatureSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IReadingsService
{
    Task<Components> GetReadings();
    Task<List<bool>> ComponentsExisting(); 
    Task InsertCPU(CPU cpu);
    Task InsertMobo(Motherboard mobo);
    Task InsertMemory(Memory memory);
    Task InsertCurrentReadings(Components components);
    //TemperatureDetails GetTempDetails();
}