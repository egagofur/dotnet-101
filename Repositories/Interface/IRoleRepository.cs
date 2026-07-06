using WarehouseApi.Enums;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories.Interface;

public interface IRoleRepository
{
    Task<IEnumerable<Roles>> GetAllAsync();

    Task<Roles?> GetByNameAsync(RolesNameEnum name);
}