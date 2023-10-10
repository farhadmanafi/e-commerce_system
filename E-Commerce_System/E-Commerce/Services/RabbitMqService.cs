using System;
using E_Commerce.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace E_Commerce.Services
{ public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(string hostName, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void PublishOrderMessage(Order order, string idempotentKey)
        {
            var message = JsonConvert.SerializeObject(order);
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Ensure messages are not lost even if RabbitMQ server restarts
            properties.Headers = new Dictionary<string, object>
            {
                { "IdempotentKey", idempotentKey }
            };

            _channel.BasicPublish(exchange: "",
                routingKey: "order_queue",
                basicProperties: properties,
                body: body);
        }

        public void SubscribeToOrderQueueWithRetry(int maxRetries, TimeSpan retryInterval, Action<Order, string> callback)
        {
            // (Code for retry logic as provided in the previous response)
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
