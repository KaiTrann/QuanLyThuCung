namespace QuanLyThuCung.Core.Models
{
    /// <summary>
    /// Represents an order in the pet shop
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string Notes { get; set; } = string.Empty;
    }
}
