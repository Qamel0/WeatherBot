using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherBot.Services
{
    public class Bot
    {
        private readonly ITelegramBotClient _botClient;

        public Bot(string token)
        {
            _botClient = new TelegramBotClient(token);
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

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update,
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

                            var chat = message.Chat;

                            switch (message.Type)
                            {
                                case MessageType.Text:
                                    {
                                        if (message.Text == "/start")
                                        {
                                            await botClient.SendMessage(
                                                chat.Id,
                                                "Список комманд:\n" +
                                                "Просмотр погоды по введённому городу: /weather\n");

                                            var replyKeyboard = new ReplyKeyboardMarkup(
                                                new List<KeyboardButton[]>()
                                                {
                                                    new KeyboardButton[]
                                                    {
                                                        new KeyboardButton("Weather")
                                                    },
                                                })

                                            {
                                                ResizeKeyboard = true,
                                            };

                                            await botClient.SendMessage(
                                                chat.Id,
                                                "Можно так же использовать открывающуюся кнопку внизу экрана",
                                                replyMarkup: replyKeyboard);

                                            return;
                                        }

                                        if (message.Text == "/weather")
                                        {
                                            
                                        }
                                        else
                                        {
                                            await botClient.SendMessage(
                                            chat.Id,
                                            "Указанной команды не существует!");
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
                Console.WriteLine(ex.ToString());
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
