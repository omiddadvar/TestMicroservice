using InventoryService.Application.Services;
using InventoryService.Domain.Abstractions;
using InventoryService.Infrastructure.Data;
using InventoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using RabbitMQ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInventoryInfrastructure(
            this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<InventoryContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<InventoryAppService>();

            return services;
        }
        public static IServiceCollection AddInventoryMessaging(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost", "/", h => { }); // configure credentials if needed

                    cfg.ReceiveEndpoint("order-created-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                    });
                });
            });

            return services;
        }
    }
}
