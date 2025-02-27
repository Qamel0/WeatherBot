namespace WeatherBot.Services
{
    public class BotHostedService : BackgroundService
    {
        private readonly Bot _bot;

        public BotHostedService(Bot bot)
        {
            _bot = bot;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _bot.StartReceiving();
            return Task.CompletedTask;
        }
    }
}
