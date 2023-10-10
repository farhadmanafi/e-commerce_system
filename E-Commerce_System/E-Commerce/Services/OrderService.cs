using E_Commerce.Configuration;
using E_Commerce.Models;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace E_Commerce.Services
{

    /// <summary>
    /// n this example, the OrderService handles placing orders using the Outbox Pattern,
    /// ensuring that the order details are stored in the outbox table within a transaction.
    /// </summary>
    public class OrderService
    {
        private readonly OutboxDbContext _dbContext;
        private readonly RabbitMqService _rabbitMqService;

        public OrderService(OutboxDbContext dbContext, RabbitMqService rabbitMqService)
        {
            _dbContext = dbContext;
            _rabbitMqService = rabbitMqService;
        }

        public void PlaceOrder(Order order, string idempotentKey)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Store order details in the outbox table
                var outboxMessage = new OutboxMessage
                {
                    MessageType = "OrderPlaced",
                    MessageContent = Newtonsoft.Json.JsonConvert.SerializeObject(order),
                    IdempotentKey = idempotentKey,
                    Timestamp = DateTime.UtcNow
                };

                _dbContext.OutboxMessages.Add(outboxMessage);
                _dbContext.SaveChanges();

                transaction.Commit();
            }

            // Publish order message to RabbitMQ for processing
            _rabbitMqService.PublishOrderMessage(order, idempotentKey);
        }
    }
}
