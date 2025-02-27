using WeatherBot.Interfaces;

namespace WeatherBot.Services
{
    public class DataValidateService : IDataValidateService
    {
        public async Task<bool> CheckCityName(string city)
        {
            return await Task.Run(() =>
            {
                foreach (char c in city)
                {
                    if (!char.IsLetter(c))
                    {
                        return false;
                    }
                }
                return true;
            });
            
        }
    }
}
