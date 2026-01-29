namespace QuanLyThuCung.Core.Models
{
    /// <summary>
    /// Represents a product (pet supplies, food, toys, etc.) in the pet shop
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public bool IsActive { get; set; }
    }
}
