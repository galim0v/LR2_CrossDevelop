using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Galimov_CrossPlatform_LR2_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        [Authorize]
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            return Ok(new { Success = true, Message = "Данные для авторизованных пользователей" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public IActionResult AddData([FromBody] string data)
        {
            return Ok(new { Success = true, Message = "Данные добавлены" });
        }
    }
}
