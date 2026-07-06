using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<Users>> GetAllAsync();
    Task<Users?> GetByIdAsync(Guid id);
    Task<Users?> GetByEmailAsync(string email);
    Task<Users> AddAsync(Users user);
    Task<bool> UpdateAsync(Users user);
    Task<bool> DeleteAsync(Guid id);
}
