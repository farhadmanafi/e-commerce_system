namespace E_Commerce.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string IdempotentKey { get; set; }
    }
}
