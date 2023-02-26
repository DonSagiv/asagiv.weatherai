using Quartz;

namespace asagiv.weatherai.service
{
    internal class GrabWeatherJob : IJob
    {
        #region Fields
        private readonly WeatherDataApiClient _weatherDataApiClient;
        private readonly WeatherDataRepository _weatherDataRepository;
        #endregion

        #region Constructor
        public GrabWeatherJob(WeatherDataApiClient weatherDataApiClient, WeatherDataRepository weatherDataRepository)
        {
            _weatherDataApiClient = weatherDataApiClient;
            _weatherDataRepository = weatherDataRepository;
        }
        #endregion

        #region Methods
        public async Task Execute(IJobExecutionContext context) 
        {
            var weatherData = await _weatherDataApiClient.RequestCurrentWeatherAsync();

            if (weatherData != null)
            {
                await _weatherDataRepository.InsertWeatherDataAsync(weatherData);
            }
        }
        #endregion
    }
}
