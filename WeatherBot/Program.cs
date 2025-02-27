using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;
using System.Reflection;
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
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddScoped<IDbConnection>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new SqlConnection(connectionString);
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRequestRepository, RequestRepository>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();

            builder.Services.AddHttpClient<OpenWeatherService>();
            
            builder.Services.AddSingleton<Bot>(provider =>
            {
                var botToken = builder.Configuration["ExternalServices:TelegramBotToken"]
                    ?? throw new ArgumentNullException("TelegramBotToken is missing in the configuration");
                var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                return new Bot(botToken, scopeFactory);
            });

            var app = builder.Build();

            var bot = app.Services.GetRequiredService<Bot>();
            bot.StartReceiving();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }

    }


}
