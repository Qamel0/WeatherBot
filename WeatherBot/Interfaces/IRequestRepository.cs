using WeatherBot.Dto;

namespace WeatherBot.Interfaces
{
    public interface IRequestRepository
    {
        Task<bool> AddRequest(RequestDto request);
        Task<bool> RequestExists(int requestId);
    }
}
