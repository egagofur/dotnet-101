using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;

    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }
}