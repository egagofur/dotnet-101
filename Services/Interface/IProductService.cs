using WarehouseApi.Models;

namespace WarehouseApi.Services.Interface;

public interface IProductService
{
    Task<Products?> GetByIdAsync(Guid id);
}