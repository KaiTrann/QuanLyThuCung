using QuanLyThuCung.Core.Models;

namespace QuanLyThuCung.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for managing customers
    /// </summary>
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer? GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        List<Customer> SearchCustomers(string keyword);
    }
}
