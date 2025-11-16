using InventoryManagementAPI.Models;
using InventoryManagementAPI.Repositories.Interfaces;
using InventoryManagementAPI.Services.Interfaces;

namespace InventoryManagementAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public (IEnumerable<Product> Products, int TotalCount) GetProducts(string? name, int page, int pageSize)
        {
            var result = _productRepository.GetFilteredProductsWithCount(name, page, pageSize);
            return result;
        }


        public bool AddProduct(Product product)
        {
            return _productRepository.AddProduct(product);
        }

        public bool UpdateProduct(int id, Product product)
        {
            return _productRepository.UpdateProduct(product);
        }

        public bool DeleteProduct(int id)
        {
            return _productRepository.DeleteProduct(id);
        }
    }
}
