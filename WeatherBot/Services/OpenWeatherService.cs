using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherBot.Interfaces;
using WeatherBot.WeatherModels;

namespace WeatherBot.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        public OpenWeatherService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["ExternalServices:OpenWeatherMapKey"]
                ?? throw new ArgumentNullException("OpenWeather API key is missing in configuration.");
            _httpClient = httpClient;
        }

        public async Task<WeatherResponseModel?> GetWeather(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(jsonResponse);

                return new WeatherResponseModel
                {
                    City = json["name"]?.ToString() ?? "Unknown",
                    Temperature = json["main"]?["temp"]?.ToObject<float>() ?? 0,
                    Cloudiness = json["clouds"]?["all"]?.ToObject<int>() ?? 0,
                    Humidity = json["main"]?["humidity"]?.ToObject<int>() ?? 0
                };
            }

            return null;
        }
    }
}
