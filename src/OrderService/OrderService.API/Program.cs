using MassTransit;
using OrderService.Application.Commands;
using RabbitMQ;
using Shared.Common.Models;
using Shared.Logging.Extensions;
using OrderService.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureSerilog();  // <-- This calls your extension method

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOrderApplicationDI();


builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        // Load RabbitMQ settings from appsettings.json
        var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

        cfg.Host(rabbitMqSettings.Host, "/", h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        // Configure endpoints automatically
        cfg.ConfigureEndpoints(context);

        // Optional: Retry policy
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePrometheusMetrics();  // <-- This calls your extension method

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
