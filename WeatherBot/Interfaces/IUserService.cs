using WeatherBot.Dto;

namespace WeatherBot.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddNewUser(long id, string? name);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto?> GetUserWithRequests(long id);
    }
}
