using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Domain.Models
{
    public class InventoryItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }

        public void Decrease(int quantity)
        {
            if (Quantity < quantity)
                throw new InvalidOperationException("Insufficient inventory.");
            Quantity -= quantity;
        }

        public void Increase(int quantity) => Quantity += quantity;
    }
}
