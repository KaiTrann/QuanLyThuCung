namespace QuanLyThuCung.Core.Models
{
    /// <summary>
    /// Represents a customer of the pet shop
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateRegistered { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Pet> Pets { get; set; } = new List<Pet>();
    }
}
