using WeatherBot.WeatherModels;

namespace WeatherBot.Interfaces
{
    public interface IOpenWeatherService
    {
        Task<WeatherResponseModel?> GetWeather(string city);
    }
}
