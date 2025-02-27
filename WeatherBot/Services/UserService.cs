using Telegram.Bot.Types;
using WeatherBot.Dto;
using WeatherBot.Interfaces;

namespace WeatherBot.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddNewUser(long id, string? name)
        {
            UserDto user = new UserDto
            {
                TelegramId = id,
                TelegramName = name
            };
            
            return await _userRepository.AddUser(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers() => await _userRepository.GetAllUsers();

        public async Task<UserDto?> GetUserWithRequests(long id) => await _userRepository.GetUserWithRequests(id);
    }
}
