using WeatherBot.Dto;
using WeatherBot.Interfaces;
using WeatherBot.Repository;

namespace WeatherBot.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> AddNewRequst(long userId, string city)
        {
            RequestDto request = new RequestDto
            {
                UserId = userId,
                RequestedCity = city
            };

            return await _requestRepository.AddRequest(request);
        }
    }
}
