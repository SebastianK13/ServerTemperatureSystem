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
        private readonly IReadingsService _readings;
        private readonly Temperatures _temperatures;
        private readonly Usage _usage;
        public MonitorController(IReadingsService readings)
        {
            _readings = readings;
            _temperatures = new Temperatures();
            _usage = new Usage();
        }

        [HttpPost]
        public async Task<IActionResult> CurrentReadings()
        {
             Components components = _temperatures.GetCurrentTemps();
            _usage.SetUsage(ref components); 
            DateTime date = DateTime.Now;
            date.AddSeconds(-date.Second);
            date.AddMilliseconds(-date.Millisecond);

            ComponentsViewModel vm = 
                new ComponentsViewModel(
                    await _readings.GetReadings(TimeService.Last20Minutes()));

                vm.CPU.CurrentUsage = components.CPU.UsageReadings
                    .Select(u => u.Usage).FirstOrDefault();
                vm.Memory.CurrentUsage = components.Memory.UsageReadings
                    .Select(u => u.Usage).FirstOrDefault();

            return Json(vm);
        }
        public async Task<IActionResult> MainPage()
        {
            await IsComponentsInDb();
            var model = await _readings.GetReadings(TimeService.Last20Minutes());

            ComponentsViewModel components = 
                new ComponentsViewModel(model);

            return View(components);
        }
        private async Task IsComponentsInDb()
        {
            var existings = await _readings.ComponentsExisting();
            Components components = _temperatures.GetSystemTemps();
            components.Memory = _usage.SetMemoryParams(); 
            
            if(!existings[0])
                await _readings.InsertCPU(components.CPU);
            
            if(!existings[1])
                await _readings.InsertMobo(components.MB);

            if(!existings[2])
                await _readings.InsertMemory(components.Memory);
        }
    }
}