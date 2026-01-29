namespace QuanLyThuCung.Core.Models
{
    /// <summary>
    /// Represents a pet in the pet shop
    /// </summary>
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
