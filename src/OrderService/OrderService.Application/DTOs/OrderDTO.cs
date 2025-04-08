using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public record class OrderDTO
    (
        Guid Id,
        string Status,
        IEnumerable<OrderItemDTO> Items);
}
