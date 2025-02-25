using WeatherBot.Dto;
using WeatherBot.Interfaces;

namespace WeatherBot.Services
{
    public class BotService : IBotService
    {
        private readonly IUserRepository _userRepository;

        public BotService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddNewUser(UserDto user) => await _userRepository.AddUser(user);
    }
}
