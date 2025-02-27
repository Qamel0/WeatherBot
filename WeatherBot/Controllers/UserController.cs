using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherBot.Dto;
using WeatherBot.Interfaces;
using WeatherBot.Services;
using WeatherBot.WeatherModels;

namespace WeatherBot.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOpenWeatherService _openWeatherService;
        private readonly Bot _bot;
        public UserController(IUserService userService, IOpenWeatherService openWeatherService,
            Bot bot)
        {
            _userService = userService;
            _openWeatherService = openWeatherService;
            _bot = bot;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserWithRequests(long userId)
        {
            UserDto? user = await _userService.GetUserWithRequests(userId);
            if(user == null)
            {
                return NotFound($"User with id: {userId} not found");
            }

            return Ok(user);
        }

        [HttpPost("sendWeatherToAll")]
        public async Task<IActionResult> SendWeatherToAllUsers([FromQuery] string city)
        {
            WeatherResponseModel? weather = await _openWeatherService.GetWeather(city);
            if(weather == null)
            {
                return NotFound($"City name: {city} not found");
            }

            await _bot.SendWeatherToAllUsers(weather, city);

            return Ok($"Message delivered successfully");
        }
    }
}
