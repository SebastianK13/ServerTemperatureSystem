using System;
using System.ComponentModel.DataAnnotations;

namespace ServerTemperatureSystem.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username field is required")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password field is required")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}