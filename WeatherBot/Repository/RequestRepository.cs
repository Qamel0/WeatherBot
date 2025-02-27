using Dapper;
using System.Data;
using Telegram.Bot.Types;
using WeatherBot.Dto;
using WeatherBot.Interfaces;

namespace WeatherBot.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IDbConnection _connection;
        public RequestRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> AddRequest(RequestDto request)
        {
            if (request == null)
            {
                return false;
            }

            string sql = "INSERT INTO WeatherHistory (UserId, RequestedCity) VALUES (@userId, @RequestedCity)";
            return await _connection.ExecuteAsync(sql, request) > 0;
        }

        public async Task<bool> RequestExists(int requestId)
        {
            string sql = $"SELECT COUNT(1) FROM WeatherHistory WHERE Id = @Id";
            return await _connection.ExecuteScalarAsync<int>(sql, new { Id = requestId }) >= 1;
        }
    }
}
