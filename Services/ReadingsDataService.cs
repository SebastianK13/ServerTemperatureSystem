using System;
using ServerTemperatureSystem.EFCoreDbContext;
using ServerTemperatureSystem.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ReadingsDataService : IReadingsService
{
    private readonly AppParamsDbContext _context;
    public ReadingsDataService(AppParamsDbContext context)
    {
        _context = context;
    }
    public async Task<Components> GetReadings()
    {
        Components components = new Components();
        components.CPU = await _context.CPU.FirstOrDefaultAsync();
        components.MB = await _context.Mobo.FirstOrDefaultAsync();
        components.Memory = await _context.Memory.FirstOrDefaultAsync();

        return components;
    }
    public async Task<List<bool>> ComponentsExisting()
    {
        List<bool> existings = new List<bool>();
        existings.Add(await _context.CPU.AnyAsync());
        existings.Add(await _context.Mobo.AnyAsync());
        existings.Add(await _context.Memory.AnyAsync());

        return existings;
    }
    public async Task InsertCPU(CPU cpu)
    {
        bool coresInserted = await InsertCores(cpu.Cores.ToList());
        if (coresInserted)
            _context.CPU.Add(cpu);

        await _context.SaveChangesAsync();
    }
    private async Task<bool> InsertCores(List<Core> cores)
    {
        _context.Cores.AddRange(cores);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task InsertMobo(Motherboard mobo)
    {
        _context.Mobo.Add(mobo);
        await _context.SaveChangesAsync();
    }
    public async Task InsertMemory(Memory memory)
    {
        _context.Memory.Add(memory);
        await _context.SaveChangesAsync();
    }
    public async Task InsertCurrentReadings(Components components)
    {
        var cpu = await _context.CPU.FirstOrDefaultAsync();
        var mobo = await _context.Mobo.FirstOrDefaultAsync();
        var memory = await _context.Memory.FirstOrDefaultAsync();
        var cores = await _context.Cores.ToListAsync();
        components.CPU.TemperatureReadings.FirstOrDefault().CPU = cpu;
        components.CPU.UsageReadings.FirstOrDefault().CPU = cpu;
        components.MB.TemperatureReadings.FirstOrDefault().Mobo = mobo;
        components.Memory.UsageReadings.FirstOrDefault().Memory = memory;

        for(int i = 0; i < components.CPU.Cores.Count(); i++)
        {
            components.CPU.Cores[i].TemperatureReadings.FirstOrDefault().Core = cores[i];
            components.CPU.Cores[i].UsageReadings.FirstOrDefault().Core = cores[i];
            await _context.TemperatureDetails
                .AddAsync(components.CPU.Cores[i].TemperatureReadings.FirstOrDefault());
            await _context.UsageDetails.AddAsync(components.CPU.Cores[i].UsageReadings.FirstOrDefault());
        }

        await _context.TemperatureDetails
            .AddAsync(components.CPU.TemperatureReadings.FirstOrDefault());
        await _context.UsageDetails
            .AddAsync(components.CPU.UsageReadings.FirstOrDefault());
        await _context.TemperatureDetails
            .AddAsync(components.MB.TemperatureReadings.FirstOrDefault());
        await _context.UsageDetails
            .AddAsync(components.Memory.UsageReadings.FirstOrDefault());

        await _context.SaveChangesAsync();
    }
    public async Task RemoveReadingsOlderThan24h()
    {
        DateTime date = DateTime.Now.AddHours(-24);
        var tempReadings = await _context.TemperatureDetails.Where(d => d.Date < date).ToListAsync();
        var usageReadings = await _context.UsageDetails.Where(d => d.Date < date).ToListAsync();

        _context.TemperatureDetails.RemoveRange(tempReadings);
        _context.UsageDetails.RemoveRange(usageReadings);

        await _context.SaveChangesAsync();
    }
}