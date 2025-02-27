using WeatherBot.Dto;

namespace WeatherBot.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddNewUser(long id, string? name);
        Task<UserDto> GetUserWithRequests(long id);
    }
}
