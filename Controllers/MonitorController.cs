using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerTemperatureSystem.Services;
using ServerTemperatureSystem.Models;

namespace ServerTemperatureSystem.Controllers
{
    public class MonitorController : Controller
    {
        private Components components; 
        private readonly IReadingsService _readings;
        private readonly Temperatures _temperatures;
        private readonly Usage _usage;
        public MonitorController(IReadingsService readings)
        {
            _readings = readings;
            _temperatures = new Temperatures();
            _usage = new Usage();
        }

        public async Task<IActionResult> Index()
        {
            await IsComponentsInDb();
            return View();
        }
        public async Task<IActionResult> MainPage()
        {
            var results = await _readings.GetReadings();

            return View(results);
        }
        private async Task IsComponentsInDb()
        {
            var existings = await _readings.ComponentsExisting();
            Components components = _temperatures.GetSystemTemps();
            _usage.SetUsage(ref components); 
            
            if(!existings[0])
                await _readings.InsertCPU(components.CPU);
            
            if(!existings[1])
                await _readings.InsertMobo(components.MB);

            if(!existings[2])
                await _readings.InsertMemory(components.Memory);
        }
    }
}