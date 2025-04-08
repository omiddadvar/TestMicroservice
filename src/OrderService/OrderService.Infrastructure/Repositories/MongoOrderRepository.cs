using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Models;
using OrderService.Infrastructure.Models;

namespace OrderService.Infrastructure.Repositories
{
    public class MongoOrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public MongoOrderRepository(IMongoDatabase database, IOptions<MongoDBSettings> options)
        {
            _collection = database.GetCollection<Order>(options.Value.CollectionName);
        }

        public async Task AddAsync(Order order)
            => await _collection.InsertOneAsync(order);

        public async Task<Order> GetByIdAsync(Guid id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> UpdateAsync(Order order)
        {
            var result = await _collection.ReplaceOneAsync(
                x => x.Id == order.Id,
                order);

            return result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

    }
}
