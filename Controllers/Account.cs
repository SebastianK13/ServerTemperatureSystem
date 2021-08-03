using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ServerTemperatureSystem.Services;
using ServerTemperatureSystem.Models.ViewModels;

namespace ServerTemperatureSystem.Controllers
{
    [Authorize]
    public class Account:Controller
    {
        private readonly IIdentityService _identity;
        public Account(IIdentityService identity) =>
            _identity = identity;
        [AllowAnonymous]
        public async Task<IActionResult> LoginPage(string returnUrl = "/Monitor/MainPage")
        {
/*             LoginModel model = new LoginModel
            {
                Login = "SDMAdmin",
                Password = "Jesiotra156^!"
            };
            bool result = await _identity.SignUpAsync(model); */
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPage(LoginModel loginData, string returnUrl = "/Monitor/MainPage")
        {
            if(ModelState.IsValid)
            {
                if(await _identity.LoginAsync(loginData))
                    return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "Incorect username or password");
            return View(loginData);
        }
        public async Task<RedirectResult> Logout()
        {
            await _identity.LogoutAsync();
            return Redirect("/Account/LoginPage");
        }
        [AllowAnonymous]
        public RedirectResult Login() =>
            Redirect("/Account/LoginPage");
    }

}