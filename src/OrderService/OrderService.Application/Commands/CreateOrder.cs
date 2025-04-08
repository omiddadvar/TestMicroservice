using MassTransit;
using MediatR;
using OrderService.Application.DTOs;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Models;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events;
using static OrderService.Application.Commands.CreateOrder;


namespace OrderService.Application.Commands
{
    public static class CreateOrder
    {
        public record Command(
            string CustomerId,
            IList<OrderItemDTO> Items
        ) : IRequest<OrderDTO>;

        public class Handler : IRequestHandler<Command, OrderDTO>
        {
            private readonly IOrderRepository _repository;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IOrderRepository repository, IPublishEndpoint publishEndpoint)
            {
                _repository = repository;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<OrderDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var orderItems = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList();

                var order = Order.Create(request.CustomerId, orderItems);
                await _repository.AddAsync(order);

                var OrderItemsContract = orderItems.Select(
                        x => new OrderItemContractDTO
                        {
                             ProductId = x.ProductId,
                             ProductName = x.ProductName,
                             Quantity = x.Quantity
                        }
                    ).ToArray();

                // Publish domain event
                await _publishEndpoint.Publish(new OrderCreatedEvent(order.Id, order.CustomerId , OrderItemsContract, DateTime.UtcNow));

                return new OrderDTO(order.Id, order.Status.ToString(), request.Items);
            }
        }
    }
}
