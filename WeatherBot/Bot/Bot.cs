using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Dto;
using WeatherBot.Interfaces;

namespace WeatherBot.Services
{
    public class Bot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;

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
            Console.WriteLine($"{me.FirstName} запущен!");

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

                            var user = message.From;

                            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                            UserDto userDto = new UserDto
                            {
                                TelegramId = user.Id,
                                TelegramName = user.Username
                            };

                            await _scopeFactory
                                .CreateScope()
                                .ServiceProvider
                                .GetRequiredService<IBotService>()
                                .AddNewUser(userDto);

                            var chat = message.Chat;

                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        if (message.Text == "/start")
                                        {
                                            await botClient.SendMessage(
                                                chat.Id,
                                                "Привіт! Вас вітає помічник для перегляду погоди.\n" +
                                                "Для навігації використовуйте меню. Воно знаходиться знизу👇");


                                            await botClient.SetMyCommands(new[]
                                            {
                                                new BotCommand { Command = "weather", Description = "Погода за містом" },
                                            });

                                            return;
                                        }

                                        if (message.Text == "/weather")
                                        {
                                            
                                        }
                                        else
                                        {
                                            await botClient.SendMessage(
                                            chat.Id,
                                            "Заданої команди не існує!");
                                        }

                                        return;
                                    }

                                default:
                                    {
                                        await botClient.SendMessage(
                                            chat.Id,
                                            "Используй только текст!");
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
    }
}
