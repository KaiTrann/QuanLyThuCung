using QuanLyThuCung.Core.Models;

namespace QuanLyThuCung.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for managing products
    /// </summary>
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        List<Product> GetProductsByCategory(string category);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        void UpdateStock(int productId, int quantity);
        List<Product> SearchProducts(string keyword);
    }
}
