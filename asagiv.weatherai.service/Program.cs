using asagiv.weatherai.service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<WeatherDataRepository>();
        services.AddSingleton<WeatherDataApiClient>();
        services.AddSingleton<JobFactory>();
    })
    .Build();

host.Run();
