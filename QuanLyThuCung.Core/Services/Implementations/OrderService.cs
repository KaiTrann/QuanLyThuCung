using QuanLyThuCung.Core.Models;
using QuanLyThuCung.Core.Services.Interfaces;

namespace QuanLyThuCung.Core.Services.Implementations
{
    /// <summary>
    /// Implementation of order management service
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new();
        private int _nextId = 1;

        public List<Order> GetAllOrders()
        {
            return _orders.ToList();
        }

        public Order? GetOrderById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _orders.Where(o => o.CustomerId == customerId).ToList();
        }

        public void CreateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            if (order.OrderItems == null || order.OrderItems.Count == 0)
                throw new ArgumentException("Order must contain at least one item", nameof(order));
                
            order.Id = _nextId++;
            order.OrderDate = DateTime.Now;
            order.TotalAmount = CalculateOrderTotal(order);
            _orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            var existingOrder = GetOrderById(order.Id);
            if (existingOrder == null)
                throw new InvalidOperationException($"Order with ID {order.Id} not found");
                
            order.TotalAmount = CalculateOrderTotal(order);
            var index = _orders.IndexOf(existingOrder);
            _orders[index] = order;
        }

        public void CancelOrder(int id)
        {
            var order = GetOrderById(id);
            if (order == null)
                throw new InvalidOperationException($"Order with ID {id} not found");
                
            order.Status = "Cancelled";
        }

        public decimal CalculateOrderTotal(Order order)
        {
            return order.OrderItems?.Sum(item => item.Subtotal) ?? 0;
        }
    }
}
