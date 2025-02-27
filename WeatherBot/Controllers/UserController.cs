using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherBot.Interfaces;

namespace WeatherBot.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserWithRequests(long userId)
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
