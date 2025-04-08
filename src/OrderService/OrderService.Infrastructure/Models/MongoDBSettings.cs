using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; } // Optional if you have specific collections

        // You can add other MongoDB-specific settings:
        public int ConnectionTimeoutMs { get; set; } = 30000;
        public int SocketTimeoutMs { get; set; } = 30000;
        public int ServerSelectionTimeoutMs { get; set; } = 30000;
        public int MaxConnectionPoolSize { get; set; } = 100;
    }
}
