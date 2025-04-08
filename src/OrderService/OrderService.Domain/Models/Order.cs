using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models
{      
    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        private List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        // Domain behaviors
        public static Order Create(string customerId, List<OrderItem> items)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                _items = items
            };
        }

        public void MarkAsPaid() => Status = OrderStatus.Paid;
    }

    public enum OrderStatus { Pending, Paid, Shipped, Cancelled }
}
