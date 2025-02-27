namespace WeatherBot.Interfaces
{
    public interface IRequestService
    {
        Task<bool> AddNewRequst(long userId, string city);
    }
}
