using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ServerTemperatureSystem.Controllers
{
    public class Account:Controller
    {
        public IActionResult LoginPage()
        {
            return View();
        }
    }

}