using NotificationService.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics => metrics
        .AddMeter("NotificationService"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
