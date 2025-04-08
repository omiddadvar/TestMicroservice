using InventoryService.Application.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly InventoryAppService _service;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(InventoryAppService service, ILogger<OrderCreatedConsumer> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation("Received OrderCreated: {OrderId}", context.Message.OrderId);
            await _service.DecreaseInventoryAsync(context.Message.Items);
        }
    }
}
