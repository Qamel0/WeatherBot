﻿namespace WeatherBot.WeatherModels
{
    public class WeatherResponse
    {
        public string City { get; set; } = null!;
        public float Temperature { get; set; }
        public int Cloudiness { get; set; }
        public int Humidity { get; set; }
    }
}
