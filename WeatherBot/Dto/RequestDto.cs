namespace WeatherBot.Dto
{
    public class RequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RequestedCity { get; set; } = null!;
        public DateTime? RequestTime { get; set; }
    }
}
