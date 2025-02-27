using WeatherBot.WeatherModels;

namespace WeatherBot.Interfaces
{
    public interface IOpenWeatherService
    {
        Task<WeatherResponse?> GetWeather(string city);
    }
}
