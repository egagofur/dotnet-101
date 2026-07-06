using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Enums;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Roles>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Roles?> GetByNameAsync(RolesNameEnum name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}