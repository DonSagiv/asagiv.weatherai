using Quartz;
using ILogger = Serilog.ILogger;

namespace asagiv.weatherai.service
{
    internal class GrabWeatherJob : IJob
    {
        #region Fields
        private readonly ILogger? _logger;
        private readonly WeatherDataApiClient _weatherDataApiClient;
        private readonly WeatherDataRepository _weatherDataRepository;
        #endregion

        #region Constructor
        public GrabWeatherJob(WeatherDataApiClient weatherDataApiClient, WeatherDataRepository weatherDataRepository, ILogger? logger = null)
        {
            _logger = logger;

            _logger?.Information("Initializing GrabWeatherJob.");

            _weatherDataApiClient = weatherDataApiClient;
            _weatherDataRepository = weatherDataRepository;
        }
        #endregion

        #region Methods
        public async Task Execute(IJobExecutionContext context) 
        {
            try
            {
                // Obtain the data from the web API.
                var weatherData = await _weatherDataApiClient.RequestCurrentWeatherAsync();

                // Log if no weather data obtained.
                if (weatherData == null)
                {
                    _logger?.Error("Weather data optained from client was null.");

                    return;
                }

                // Append the weather data to the MongoDB repository.
                await _weatherDataRepository.InsertWeatherDataAsync(weatherData);
            }
            catch(Exception ex)
            {
                _logger?.Error("Error in task execution", ex);
            }
        }
        #endregion
    }
}
