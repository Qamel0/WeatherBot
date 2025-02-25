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

        public async Task<bool> UserExists(long userId)
        {
            string sql = $"SELECT COUNT(1) FROM Users WHERE TelegramId = @TelegramId";
            return await _connection.ExecuteScalarAsync<int>(sql, new { TelegramId = userId }) >= 1;
        }
    }
}
