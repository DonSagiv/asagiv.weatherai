namespace asagiv.weatherai.service
{
    public class Worker : BackgroundService
    {
        #region Fields
        private readonly WeatherDataApiClient _weatherDataApiClient;
        private readonly WeatherDataRepository _weatherDataRepository;
        #endregion

        public Worker(WeatherDataApiClient weatherDataApiClient, WeatherDataRepository weatherDataCollection)
        {
            _weatherDataApiClient = weatherDataApiClient;
            _weatherDataRepository = weatherDataCollection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var weatherData = await _weatherDataApiClient.RequestCurrentWeatherAsync();

            if (weatherData != null)
            {
                await _weatherDataRepository.InsertWeatherDataAsync(weatherData);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}