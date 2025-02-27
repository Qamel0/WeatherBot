namespace WeatherBot.Interfaces
{
    public interface IDataValidateService
    {
        Task<bool> CheckCityName(string city);
    }
}
