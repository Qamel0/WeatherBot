namespace WeatherBot.Dto
{
    public class UserDto
    {
        public long TelegramId { get; set; }
        public string TelegramName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
