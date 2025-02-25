using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("{userId}")]
        public IActionResult GetUserWithRequests(int userId)
        {
            return Ok($"Пользователь с id: {userId}");
        }

        [HttpPost("sendWeatherToAll")]
        public IActionResult SendWeatherToAllUsers([FromQuery] string city)
        {
            return Ok($"Город с названием: {city}");
        }
    }
}
