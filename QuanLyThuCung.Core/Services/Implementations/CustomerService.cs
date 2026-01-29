using QuanLyThuCung.Core.Models;
using QuanLyThuCung.Core.Services.Interfaces;

namespace QuanLyThuCung.Core.Services.Implementations
{
    /// <summary>
    /// Implementation of customer management service
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers = new();
        private int _nextId = 1;

        public List<Customer> GetAllCustomers()
        {
            return _customers.ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public void AddCustomer(Customer customer)
        {
            customer.Id = _nextId++;
            customer.DateRegistered = DateTime.Now;
            _customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = GetCustomerById(customer.Id);
            if (existingCustomer != null)
            {
                var index = _customers.IndexOf(existingCustomer);
                _customers[index] = customer;
            }
        }

        public void DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _customers.Remove(customer);
            }
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            return _customers.Where(c =>
                c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
    }
}
