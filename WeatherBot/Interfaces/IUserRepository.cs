using WeatherBot.Dto;

namespace WeatherBot.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUser(UserDto user);
        Task<bool> UserExists(long userId);
        Task<UserDto?> GetUser(long userId);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto?> GetUserWithRequests(long userId);
    }
}
