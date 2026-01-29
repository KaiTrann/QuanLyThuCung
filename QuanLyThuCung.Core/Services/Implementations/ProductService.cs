using QuanLyThuCung.Core.Models;
using QuanLyThuCung.Core.Services.Interfaces;

namespace QuanLyThuCung.Core.Services.Implementations
{
    /// <summary>
    /// Implementation of product management service
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();
        private int _nextId = 1;

        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        public void AddProduct(Product product)
        {
            product.Id = _nextId++;
            product.DateAdded = DateTime.Now;
            _products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = GetProductById(product.Id);
            if (existingProduct != null)
            {
                var index = _products.IndexOf(existingProduct);
                _products[index] = product;
            }
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }

        public void UpdateStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                product.StockQuantity += quantity;
            }
        }

        public List<Product> SearchProducts(string keyword)
        {
            return _products.Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
    }
}
