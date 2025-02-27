using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Dto;
using WeatherBot.Interfaces;
using WeatherBot.WeatherModels;

namespace WeatherBot.Services
{
    public class Bot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly Dictionary<long, string> _userStates = new();

        public Bot(string token, IServiceScopeFactory scopeFactory)
        {
            _botClient = new TelegramBotClient(token);
            _scopeFactory = scopeFactory;
        }

        public async void StartReceiving()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] 
                { 
                    UpdateType.Message,
                    UpdateType.CallbackQuery,
                },
                DropPendingUpdates = true
            };

            using var cts = new CancellationTokenSource();

            _botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);

            var me = await _botClient.GetMe();
            Console.WriteLine($"{me.FirstName} запущений!");

            await Task.Delay(-1);
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            if (message == null) return;
                            
                            var user = message.From;
                            if(user == null) return;

                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                            await AddNewUserToDatabase(user.Id, user.Username);

                            var chat = message.Chat;

                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        await CommandProcessing(botClient, message, chat, user.Id);
                                        return;
                                    }
                                default:
                                    {
                                        await botClient.SendMessage(
                                            chat.Id,
                                            "Я приймаю тільки команди!");
                                        return;
                                    }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task<WeatherResponse?> GetWeatherFromApi(string city)
        {
            using var scope = _scopeFactory.CreateScope();
            var weatherService = scope.ServiceProvider.GetRequiredService<IOpenWeatherService>();

            return await weatherService.GetWeather(city);
        }

        private async Task<bool> AddNewUserToDatabase(long id, string? name)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            return await userService.AddNewUser(id, name);
        }

        private async Task CommandProcessing(ITelegramBotClient botClient, Message message, Chat chat, long userId)
        {
            if(!_userStates.ContainsKey(userId))
            {
                _userStates[userId] = "default";
            }

            if (string.IsNullOrEmpty(message.Text)) return;

            switch (_userStates[userId])
            {
                case "default":
                    switch (message.Text)
                    {
                        case "/start":
                            {
                                await botClient.SendMessage(
                                chat.Id,
                                "Привіт! Вас вітає помічник для перегляду погоди.\n" +
                                "Для перегляду погоди можете використовувати команду /weather {місто}.\n" +
                                "Також ви можете використовувати меню для навігації. Воно знаходиться знизу👇");

                                await botClient.SetMyCommands(new[]
                                {
                                    new BotCommand { Command = "weather", Description = "Погода за містом" },
                                });

                                return;
                            }

                        case "/weather":
                            {
                                await botClient.SendMessage(
                                chat.Id,
                                "Введіть назву міста для отримання погоди");

                                _userStates[userId] = "responseWaiting";
                                return;
                            }

                        case string text when text.StartsWith("/weather") && text.Length > 9:
                            {
                                string city = text.Substring(9).Trim();

                                WeatherResponse? weather = await GetWeatherFromApi(city);

                                if (weather == null)
                                {
                                    await botClient.SendMessage(
                                    chat.Id,
                                    $"Не вдалося знайти вказане місто");

                                    return;
                                }

                                ///Добавление запроса в базу данных

                                await botClient.SendMessage(
                                chat.Id,
                                $"Погода у місті {city}\n" +
                                $"Температура: {(int)weather.Temperature}°\n" +
                                $"Пасмурність: {weather.Cloudiness}%\n" +
                                $"Вологість: {weather.Humidity}%");

                                return;
                            }

                        default:
                            {
                                await botClient.SendMessage(
                                chat.Id,
                                "Заданої команди не існує!");

                                return;
                            }
                    }
                case "responseWaiting":
                    {
                        string city = message.Text;

                        WeatherResponse? cityWeather = await GetWeatherFromApi(city);

                        if (cityWeather != null)
                        {
                            await botClient.SendMessage(
                                chat.Id,
                                $"Погода у місті {city}\n" +
                                $"Температура: {(int)cityWeather.Temperature}°\n" +
                                $"Пасмурність: {cityWeather.Cloudiness}%\n" +
                                $"Вологість: {cityWeather.Humidity}%");

                            _userStates[userId] = "default"; // Добавлено: возврат к состоянию по умолчанию.
                            return;
                        }

                        await botClient.SendMessage(
                            chat.Id,
                            "Не вдалося знайти вказане місто. Спробуйте ще раз.");
                        return;
                    }
            
            }
        }

    }
}
