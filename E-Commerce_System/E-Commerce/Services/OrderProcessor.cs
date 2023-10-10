namespace E_Commerce.Services
{
    /// <summary>
    /// The OrderProcessor uses the Inbox Pattern to consume orders from RabbitMQ,
    /// checks idempotency using the Idempotent Pattern, processes the order, and saves it to MongoDB. 
    /// </summary>
    public class OrderProcessor
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly OrderRepository _orderRepository;

        public OrderProcessor(RabbitMqService rabbitMqService, OrderRepository orderRepository)
        {
            _rabbitMqService = rabbitMqService;
            _orderRepository = orderRepository;
        }

        public void StartProcessingWithRetries(int maxRetries, TimeSpan retryInterval)
        {
            _rabbitMqService.SubscribeToOrderQueueWithRetry(maxRetries, retryInterval, (order, idempotentKey) =>
            {
                try
                {
                    // Check idempotency using the idempotentKey
                    if (!_orderRepository.OrderExists(idempotentKey))
                    {
                        // Process the order
                        _orderRepository.SaveOrder(order, idempotentKey);
                        Console.WriteLine($"Order processed: {order.OrderId}");
                    }
                    else
                    {
                        Console.WriteLine($"Duplicate order detected: {order.OrderId}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle specific exceptions (e.g., database errors)
                    Console.WriteLine($"Error processing order: {ex.Message}");
                    throw; // Rethrow the exception to trigger retries
                }
            });
        }
    }

}
