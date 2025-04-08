using System.Diagnostics.Metrics;

namespace NotificationService.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMeterFactory _meterFactory;
        private readonly Counter<int> _jobsProcessed;

        public Worker(ILogger<Worker> logger, IMeterFactory meterFactory)
        {
            _logger = logger;
            _meterFactory = meterFactory;
            var meter = meterFactory.Create("NotificationService");
            _jobsProcessed = meter.CreateCounter<int>("jobs_processed");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                _jobsProcessed.Add(1);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
