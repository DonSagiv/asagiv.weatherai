using Quartz;
using Quartz.Spi;

namespace asagiv.weatherai.service
{
    public class JobFactory : IJobFactory
    {
        #region Fields
        private readonly WeatherDataApiClient _weatherDataApiClient;
        private readonly WeatherDataRepository _weatherDataRepository;
        #endregion

        #region Constructor
        public JobFactory(WeatherDataApiClient weatherDataApiClient, WeatherDataRepository weatherDataCollection)
        {
            _weatherDataApiClient = weatherDataApiClient;
            _weatherDataRepository = weatherDataCollection;
        }
        #endregion

        #region Methods
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new GrabWeatherJob(_weatherDataApiClient, _weatherDataRepository);
        }

        public void ReturnJob(IJob job)
        {
            if(job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        #endregion
    }
}
