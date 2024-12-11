using Galimov_CrossPlatform_LR2_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace Galimov_CrossPlatform_LR2_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Простая проверка логина и пароля
            if (model.Username == "admin" && model.Password == "password")
            {
                var token = AuthOptions.GenerateToken(model.Username, isAdmin: true);
                return Ok(new { token });
            }
            else if (model.Username == "user" && model.Password == "password")
            {
                var token = AuthOptions.GenerateToken(model.Username, isAdmin: false);
                return Ok(new { token });
            }

            return Unauthorized("Неверное имя пользователя или пароль");
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}