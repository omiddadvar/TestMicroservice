using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderService.Domain.Abstractions;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        // In OrderService.Infrastructure/DependencyInjection.cs
        public static IServiceCollection AddOrderRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, MongoOrderRepository>();
            return services;
        }
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });

                //x.AddConsumer<OrderCreatedConsumer>();
            });

            return services;
        }
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(configuration["MongoDB:ConnectionString"]));
            services.AddScoped(sp =>
                sp.GetRequiredService<IMongoClient>().GetDatabase(configuration["MongoDB:DatabaseName"]));

            return services;
        }
    }
}
