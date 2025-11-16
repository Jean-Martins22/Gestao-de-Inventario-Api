using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        (IEnumerable<Product> Products, int TotalCount) GetFilteredProductsWithCount(string? name, int page, int pageSize);
        bool AddProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
    }
}
