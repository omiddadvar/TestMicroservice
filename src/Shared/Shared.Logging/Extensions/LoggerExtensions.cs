using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using OpenTelemetry.Resources;


namespace Shared.Logging.Extensions
{
    public static class LoggerExtensions
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration)
                   .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                   .Enrich.WithProcessId()
                   .Enrich.WithThreadId()
                   .WriteTo.Console()
                   .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                   .WriteTo.Seq(context.Configuration["Seq:ServerUrl"]);
            });
        }
        public static IServiceCollection ConfigurePrometheus(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    )
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("MassTransit")
                    );
            return services;
        }
        public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            app.UseHttpMetrics(options =>
            {
                options.RequestDuration.Enabled = true;
                options.InProgress.Enabled = true;
                options.RequestCount.Enabled = true;
            });

            // OpenTelemetry Prometheus exporter
            return app;
        }
    }
}
