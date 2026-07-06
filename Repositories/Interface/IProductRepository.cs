
using WarehouseApi.Models;

namespace WarehouseApi.Repositories.Interface;

public interface IProductRepository
{
    Task<Products?> GetByIdAsync(Guid id);

}