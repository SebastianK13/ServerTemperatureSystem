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
    private DateTime date;
    public ReadingsDataService(AppParamsDbContext context)
    {
        _context = context;
    }
    public async Task<Components> GetReadings(DateTime date)
    {
        this.date = date;
        Components c = new Components();
        c.CPU = await GetCPUAsync();
        c.MB = await GetMotherboardAsync();
        c.Memory = await GetMemoryAsync();

        return c;
    }
    private async Task<Memory> GetMemoryAsync()
    {
        Memory m = await _context.Memory.FirstOrDefaultAsync();
        m.UsageReadings = await _context.UsageDetails
            .Where(i => i.Memory.Id == m.Id && i.Date >= date)
            .OrderBy(d => d.Date)
            .ToListAsync();
        
        return m;
    }
    public async Task<double> GetTempMinValue(int id, string componentName)
    {
        switch(componentName)
        {
            case "CPU":
                return await _context.TemperatureDetails.Where(i=>i.CPU.Id == id).MinAsync(t=>t.Temperature);
            case "Mobo":
                return await _context.TemperatureDetails.Where(i=>i.Mobo.Id == id).MinAsync(t=>t.Temperature);
        }
        return 0;
    }
    public async Task<double> GetTempMaxValue(int id, string componentName)
    {
        switch(componentName)
        {
            case "CPU":
                return await _context.TemperatureDetails.Where(i=>i.CPU.Id == id).MaxAsync(t=>t.Temperature);
            case "Mobo":
                return await _context.TemperatureDetails.Where(i=>i.Mobo.Id == id).MaxAsync(t=>t.Temperature);
        }
        return 0;
    }
    public async Task<int> GetUsageMinValue(int id, string componentName)
    {
        switch(componentName)
        {
            case "CPU":
                return await _context.UsageDetails.Where(i=>i.CPU.Id == id).MinAsync(u => u.Usage);
            case "Memory":
                return await _context.UsageDetails.Where(i=>i.Memory.Id == id).MinAsync(u => u.Usage);
        }
        return 0;
    }
    public async Task<int> GetUsageMaxValue(int id, string componentName)
    {
        switch(componentName)
        {
            case "CPU":
                return await _context.UsageDetails.Where(i=>i.CPU.Id == id).MaxAsync(u => u.Usage);
            case "Memory":
                return await _context.UsageDetails.Where(i=>i.Memory.Id == id).MaxAsync(u => u.Usage);
        }
        return 0;
    }
    private async Task<Motherboard> GetMotherboardAsync()
    {
        Motherboard mobo = await _context.Mobo.FirstOrDefaultAsync();
        mobo.TemperatureReadings = await _context.TemperatureDetails
            .Where(i => i.Mobo.Id == mobo.Id && i.Date >= date)
            .OrderBy(d => d.Date)
            .ToListAsync();

        return mobo;
    }
    private async Task<CPU> GetCPUAsync()
    {
        var cores = await _context.Cores.ToListAsync();

        for (int i = 0; i < cores.Count(); i++)
        {
            cores[i].UsageReadings = await _context.UsageDetails
                .Where(c => c.Core.Id == cores[i].Id && c.Date >= date)
                .OrderBy(d => d.Date)
                .ToListAsync();

            cores[i].TemperatureReadings = await _context.TemperatureDetails
                .Where(c => c.Core.Id == cores[i].Id && c.Date >= date)
                .OrderBy(d => d.Date)
                .ToListAsync();
        }

        CPU c = await _context.CPU.FirstOrDefaultAsync();
        c.UsageReadings = await _context.UsageDetails
            .Where(i => i.CPU.Id == c.Id && i.Date >= date)
            .OrderBy(d => d.Date)
            .ToListAsync();

        c.TemperatureReadings = await _context.TemperatureDetails
            .Where(i => i.CPU.Id == c.Id && i.Date >= date)
            .OrderBy(d => d.Date)
            .ToListAsync();

        c.Cores = cores;

        return c;
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

        for (int i = 0; i < components.CPU.Cores.Count(); i++)
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