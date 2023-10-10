using E_Commerce.Models;
using MongoDB.Driver;

namespace E_Commerce.Services
{
    /// <summary>
    /// The OrderRepository handles interactions with MongoDB.
    /// </summary>
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _orderCollection = database.GetCollection<Order>("orders");
        }

        public void SaveOrder(Order order, string idempotentKey)
        {
            // Save order to MongoDB
            order.IdempotentKey = idempotentKey;
            _orderCollection.InsertOne(order);
        }

        public bool OrderExists(string idempotentKey)
        {
            // Check if order with the given idempotentKey exists in MongoDB
            var filter = Builders<Order>.Filter.Eq("IdempotentKey", idempotentKey);
            return _orderCollection.Find(filter).Any();
        }
    }
}
