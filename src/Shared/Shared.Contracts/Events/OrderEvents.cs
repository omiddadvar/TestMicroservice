using OrderService.Domain.Models;
using Shared.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public record OrderCreatedEvent(
        Guid OrderId,
        string CustomerId,
        IList<OrderItemContractDTO> Items,
        DateTime OccurredOn);

}
