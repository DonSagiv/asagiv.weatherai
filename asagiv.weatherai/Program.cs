namespace asagiv.weatherai
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var a = new WeatherApiClient(11566);

            await a.RequestCurrentWeatherAsync();
        }
    }
}