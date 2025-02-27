using WeatherBot.Dto;
using WeatherBot.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WeatherBot.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> AddUser(UserDto user)
        {
            if(user == null || await UserExists(user.TelegramId))
            {
                return false;
            }

            string sql = "INSERT INTO Users (TelegramId, TelegramName) VALUES (@TelegramId, @TelegramName)";
            return await _connection.ExecuteAsync(sql, user) > 0;
        }

        public async Task<UserDto?> GetUser(long userId)
        {
            string sql = "SELECT TelegramId, TelegramName, CreatedAt FROM Users WHERE TelegramId = @TelegramId";
            return await _connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { TelegramId = userId });
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            string sql = "SELECT TelegramId, TelegramName, CreatedAt FROM Users";
            return await _connection.QueryAsync<UserDto>(sql);
        }

        public async Task<UserDto?> GetUserWithRequests(long userId)
        {
            string sql = @"SELECT u.TelegramId, u.TelegramName, u.CreatedAt,
                           r.Id, r.UserId, r.RequestedCity, r.RequestTime
                           FROM Users u
                           LEFT JOIN WeatherHistory r ON u.TelegramId = r.UserId
                           WHERE u.TelegramId = @TelegramId";

            var userDictionary = new Dictionary<long, UserDto>();

            var user = await _connection.QueryAsync<UserDto, RequestDto, UserDto>(
                sql,
                (user, request) =>
                {
                    if (!userDictionary.TryGetValue(user.TelegramId, out var userDto))
                    {
                        userDto = new UserDto
                        {
                            TelegramId = user.TelegramId,
                            TelegramName = user.TelegramName,
                            CreatedAt = user.CreatedAt,
                            Requests = new List<RequestDto>()
                        };
                        userDictionary.Add(user.TelegramId, userDto);
                    }
                    
                    if (request != null)
                        userDto.Requests!.Add(request);

                    return userDto;
                },
                new { TelegramId = userId },
                splitOn: "Id"
                );

            return user.FirstOrDefault();
        }

        public async Task<bool> UserExists(long userId)
        {
            string sql = $"SELECT COUNT(1) FROM Users WHERE TelegramId = @TelegramId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { TelegramId = userId }) >= 1;
        }
    }
}
