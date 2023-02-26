using MongoDB.Bson.Serialization.Attributes;

namespace asagiv.weatherai.service
{
    public class WeatherData
    {
        #region Properties
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimeZone { get; set; }
        public long Epoch { get; set; }
        public double TemperatureC { get; set; }
        public bool IsDaytime { get; set; }
        public double WindSpeedKph { get; set; }
        public double WindDirectionDegrees { get; set; }
        public double AtmPressureMillibars { get; set; }
        public double PrecipitationMm { get; set; }
        public double RelativeHumidityPct { get; set; }
        public bool IsCloudy { get; set; }
        public double FeelsLikeTempC { get; set; }
        public double VisibilityKm { get; set; }
        public double UltravioletIndex { get; set; }
        public double WindGustKph { get; set; }
        public string Condition { get; set; }
        [BsonIgnore]
        public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(Epoch).DateTime.ToLocalTime();
        #endregion
    }
}
