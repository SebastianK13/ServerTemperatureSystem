using System;
using ServerTemperatureSystem.Models.ViewModels;
using System.Threading.Tasks;

namespace ServerTemperatureSystem.Services
{
    public interface IIdentityService
    {
        Task<bool> LoginAsync(LoginModel loginData);
        Task LogoutAsync();
        Task<bool> SignUpAsync(LoginModel registerData);
    }
}