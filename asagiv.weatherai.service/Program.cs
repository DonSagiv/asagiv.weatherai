using asagiv.common.Logging;
using asagiv.weatherai.service;
using Serilog.Core;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.UseSerilog();
        services.AddSingleton<WeatherDataRepository>();
        services.AddSingleton<WeatherDataApiClient>();
        services.AddSingleton<GrabWeatherJobFactory>();
    })
    .Build();

host.Run();
