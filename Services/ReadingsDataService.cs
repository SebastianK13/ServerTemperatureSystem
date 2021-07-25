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

    }
    public async Task InsertMobo(Motherboard mobo)
    {

    }
    public async Task InsertMemory(Memory memory)
    {

    }
    public async Task InsertCores(List<Core> cores)
    {

    }
}