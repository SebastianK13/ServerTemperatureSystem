using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerTemperatureSystem.Services;
using ServerTemperatureSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ServerTemperatureSystem.Controllers
{
    [Authorize]
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

            var currentReadings = await _readings.GetReadings(TimeService.Last20Minutes());

            ComponentsViewModel vm = new ComponentsViewModel(currentReadings);

                vm.CPU.CurrentUsage = components.CPU.UsageReadings
                    .Select(u => u.Usage).FirstOrDefault();
                vm.Memory.CurrentUsage = components.Memory.UsageReadings
                    .Select(u => u.Usage).FirstOrDefault();

            vm.CPU.HighestTemp = await _readings.GetTempMaxValue(currentReadings.CPU.Id, "CPU");
            vm.CPU.LowestTemp = await _readings.GetTempMinValue(currentReadings.CPU.Id, "CPU");
            vm.CPU.HighestUsage = await _readings.GetUsageMaxValue(currentReadings.CPU.Id, "CPU");
            vm.CPU.LowestUsage = await _readings.GetUsageMinValue(currentReadings.CPU.Id, "CPU");

            vm.Memory.HighestUsage = await _readings.GetUsageMaxValue(currentReadings.Memory.Id, "Memory");
            vm.Memory.LowestUsage = await _readings.GetUsageMinValue(currentReadings.Memory.Id, "Memory");

            vm.Mobo.HighestTemp = await _readings.GetTempMaxValue(currentReadings.MB.Id, "Mobo");
            vm.Mobo.LowestTemp = await _readings.GetTempMinValue(currentReadings.MB.Id, "Mobo");

            return Json(vm);
        }
        public async Task<IActionResult> MainPage()
        {
            ComponentsViewModel components = new ComponentsViewModel();
            await IsComponentsInDb();
            var model = await _readings.GetReadings(TimeService.Last20Minutes());
            if(model != null)
                components = 
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