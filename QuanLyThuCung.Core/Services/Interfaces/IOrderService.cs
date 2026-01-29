using QuanLyThuCung.Core.Models;

namespace QuanLyThuCung.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for managing orders
    /// </summary>
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order? GetOrderById(int id);
        List<Order> GetOrdersByCustomerId(int customerId);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void CancelOrder(int id);
        decimal CalculateOrderTotal(Order order);
    }
}
