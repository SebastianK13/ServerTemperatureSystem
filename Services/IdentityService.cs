using System;
using ServerTemperatureSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using ServerTemperatureSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ServerTemperatureSystem.Services
{
    public class IdentityService: IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> LoginAsync(LoginModel loginData)
        {
            IdentityUser user = await _userManager.FindByNameAsync(loginData.Login);
            if(user != null)
                if((await _signInManager.PasswordSignInAsync(user, loginData.Password, false, false)).Succeeded)
                    return true;

            return false;
        }
        [Authorize]
        public async Task LogoutAsync() =>
            await _signInManager.SignOutAsync();

        public async Task<bool> SignUpAsync(LoginModel registerData)
        {
            IdentityUser user = await _userManager.FindByNameAsync(registerData.Login);
            if(user == null)
            {
                user = new IdentityUser(registerData.Login);
                if((await _userManager.CreateAsync(user, registerData.Password)).Succeeded)
                    return true;
            }

            return false;
        }
    }
}