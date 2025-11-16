using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services.Interfaces
{
    public interface IProductService
    {
        (IEnumerable<Product> Products, int TotalCount) GetProducts(string? name, int page, int pageSize);
        bool AddProduct(Product product);
        bool UpdateProduct(int id, Product product);
        bool DeleteProduct(int id);
    }
}
