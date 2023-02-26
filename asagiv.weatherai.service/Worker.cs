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

        public Worker(GrabWeatherJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Add reset event for key pressing to cancel task.
            var quitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (s, e) =>
            {
                // When cancel key pressed, finish the task.
                quitEvent.Set();
                e.Cancel = true;
            };

            // Create the Quartz.NET scheduler factory.
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler(stoppingToken);
            await scheduler.Start(stoppingToken);

            // Job factory is used to inject API client and repository.
            scheduler.JobFactory = _jobFactory;

            // Create the job using the factory.
            var job = JobBuilder.Create<GrabWeatherJob>()
                .WithIdentity("grabWeatherJob", "asagiv")
                .Build();

            // Weather updates every 15 minutes.
            // Query 5 minutes after the hour to ensure data from latest hour is appended.
            var trigger = TriggerBuilder.Create()
                .WithIdentity("grabWeatherTrigger", "asagiv")
                .WithCronSchedule("0 5 * * * ?")
                .Build();

            // Schedule the job w/ the trigger.
            await scheduler.ScheduleJob(job, trigger, stoppingToken);

            // Wait until the cancel key is pressed.
            quitEvent.WaitOne();

            // Shut down the scheduler.
            await scheduler.Shutdown(false, stoppingToken);
        }
    }
}