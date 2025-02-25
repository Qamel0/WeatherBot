using WeatherBot.Dto;

namespace WeatherBot.Interfaces
{
    public interface IBotService
    {
        Task<bool> AddNewUser(UserDto user);
    }
}
