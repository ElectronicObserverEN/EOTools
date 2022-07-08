using ReadBattleResultService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

AppSettings.LoadSettings();

await host.RunAsync();
