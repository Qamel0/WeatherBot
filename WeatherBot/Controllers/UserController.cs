using Microsoft.AspNetCore.Mvc;
using WeatherBot.Dto;
using WeatherBot.Interfaces;
using WeatherBot.Services;
using WeatherBot.WeatherModels;

namespace WeatherBot.Controllers
{
    /// <summary>
    /// Controller for managing users and sending weather updates.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOpenWeatherService _openWeatherService;
        private readonly Bot _bot;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">Service for user operations.</param>
        /// <param name="openWeatherService">Service for retrieving weather data.</param>
        /// <param name="bot">Bot for sending messages to users.</param>
        public UserController(IUserService userService, IOpenWeatherService openWeatherService,
            Bot bot)
        {
            _userService = userService;
            _openWeatherService = openWeatherService;
            _bot = bot;
        }

        /// <summary>
        /// Get a user and their requests by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>User information along with their requests.</returns>
        /// <response code="200">The user was found successfully.</response>
        /// <response code="404">The user with the specified ID was not found.</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserWithRequests(long userId)
        {
            UserDto? user = await _userService.GetUserWithRequests(userId);
            if(user == null)
            {
                return NotFound($"User with id: {userId} not found");
            }

            return Ok(user);
        }

        /// <summary>
        /// Send weather information to all users.
        /// </summary>
        /// <param name="city">The name of the city to retrieve weather information for.</param>
        /// <returns>A success message indicating the result of the operation.</returns>
        /// <response code="200">The message was delivered successfully.</response>
        /// <response code="404">The specified city name was not found.</response>
        [HttpPost("sendWeatherToAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
