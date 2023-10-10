using System;

namespace E_Commerce.Models
{
    public class OutboxMessage
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }
        public string IdempotentKey { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
