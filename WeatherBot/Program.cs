using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Xsl;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Interfaces;
using WeatherBot.Repository;
using WeatherBot.Services;

namespace WeatherBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IDbConnection>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new SqlConnection(connectionString);
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBotService, BotService>();

            builder.Services.AddSingleton<Bot>(provider =>
            {
                var botToken = builder.Configuration["TelegramBotToken"];
                var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                return new Bot(botToken, scopeFactory);
            });

            //var botToken = builder.Configuration["TelegramBotToken"];
            //builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

            var app = builder.Build();

            var bot = app.Services.GetRequiredService<Bot>();
            bot.StartReceiving();

            //Bot bot = new Bot(botToken);
            //bot.StartReceiving();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }

    }


}
