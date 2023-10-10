# e-commerce_system
Components: Outbox Pattern, Inbox Pattern, Idempotent Pattern, MongoDB, RabbitMQ

this example that combines the Outbox Pattern, Inbox Pattern, Idempotent Pattern, MongoDB, and RabbitMQ in a C# project. This example will simulate a simple e-commerce system where orders are placed and processed.

Components:
1. Outbox Pattern:

When an order is placed, store the order details in an outbox table within the same database transaction.

2. Inbox Pattern (Polling Consumer Pattern):

Create a consumer that polls the RabbitMQ queue for new orders and processes them asynchronously.

3. Idempotent Pattern:

Use an idempotent key to ensure that order placement requests are idempotent.

4. MongoDB:

Store processed orders in MongoDB for persistence.

5. RabbitMQ:

Use RabbitMQ as the message broker for communication between services.