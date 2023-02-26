using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace asagiv.weatherai.service
{
    public class Worker : BackgroundService
    {
        #region Fields
        private readonly IJobFactory _jobFactory;
        #endregion

        public Worker(JobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();
            await scheduler.Start(stoppingToken);

            scheduler.JobFactory = _jobFactory;

            var job = JobBuilder.Create<GrabWeatherJob>()
                .WithIdentity("grabWeatherJob", "asagiv")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("grabWeatherTrigger", "asagiv")
                .WithCronSchedule("0 5 * * * ?")
                .Build();

            await scheduler.ScheduleJob(job, trigger, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}