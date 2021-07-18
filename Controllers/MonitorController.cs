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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MainPage()
        {
            Temperatures temps = new Temperatures();
            Usage usage = new Usage();
            Components results = temps.GetSystemTemps();
            usage.SetUsage(ref results);

            return View(results);
        }
    }
}