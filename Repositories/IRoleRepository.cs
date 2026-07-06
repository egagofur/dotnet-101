using WarehouseApi.Enums;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public interface IRoleRepository
{
    Task<IEnumerable<Roles>> GetAllAsync();

    Task<Roles?> GetByNameAsync(RolesNameEnum name);
}