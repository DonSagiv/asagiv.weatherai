using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace asagiv.weatherai.service
{
    public class WeatherDataJsonConverter : JsonConverter<WeatherData>
    {
        #region Statics
        private static readonly Lazy<WeatherDataJsonConverter> _lazyInstance = new(() => new WeatherDataJsonConverter());
        public static WeatherDataJsonConverter Instance => _lazyInstance.Value;
        #endregion

        #region Constructor
        private WeatherDataJsonConverter() { }
        #endregion

        #region Methods
        public override void WriteJson(JsonWriter writer, WeatherData? value, JsonSerializer serializer) => throw new NotImplementedException();

        public override WeatherData? ReadJson(JsonReader reader, Type objectType, WeatherData? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            if (jsonObject is null)
            {
                return null;
            }

            existingValue ??= new WeatherData();

            if (jsonObject["location"] is not JObject locationData)
            {
                return null;
            }

            if (jsonObject["current"] is not JObject currentData)
            {
                return null;
            }

            if (currentData["condition"] is not JObject conditionData)
            {
                return null;
            }

            existingValue.City = locationData["name"]?.ToObject<string>() ?? string.Empty;
            existingValue.State = locationData["region"]?.ToObject<string>() ?? string.Empty;
            existingValue.Country = locationData["country"]?.ToObject<string>() ?? string.Empty;
            existingValue.Latitude = locationData["lat"]?.ToObject<double>() ?? double.NaN;
            existingValue.Longitude = locationData["lon"]?.ToObject<double>() ?? double.NaN;
            existingValue.TimeZone = locationData["tz_id"]?.ToObject<string>() ?? string.Empty;
            existingValue.Epoch = currentData["last_updated_epoch"]?.ToObject<long>() ?? 0;
            existingValue.TemperatureC = currentData["temp_c"]?.ToObject<long>() ?? 0;
            existingValue.IsDaytime = currentData["is_day"]?.ToObject<bool>() ?? false;
            existingValue.Condition = conditionData["text"]?.ToObject<string>() ?? string.Empty;
            existingValue.WindSpeedKph = currentData["wind_kph"]?.ToObject<double>() ?? double.NaN;
            existingValue.WindDirectionDegrees = currentData["wind_degree"]?.ToObject<double>() ?? double.NaN;
            existingValue.AtmPressureMillibars = currentData["pressure_mb"]?.ToObject<double>() ?? double.NaN;
            existingValue.PrecipitationMm = currentData["precip_mm"]?.ToObject<double>() ?? double.NaN;
            existingValue.RelativeHumidityPct = currentData["humidity"]?.ToObject<double>() ?? double.NaN;
            existingValue.IsCloudy = currentData["cloud"]?.ToObject<bool>() ?? false;
            existingValue.FeelsLikeTempC = currentData["feelslike_c"]?.ToObject<double>() ?? double.NaN;
            existingValue.VisibilityKm = currentData["vis_km"]?.ToObject<double>() ?? double.NaN;
            existingValue.UltravioletIndex = currentData["uv"]?.ToObject<double>() ?? double.NaN;
            existingValue.WindGustKph = currentData["gust_kph"]?.ToObject<double>() ?? double.NaN;

            return existingValue;
        }
        #endregion
    }
}
