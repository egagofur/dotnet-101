using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;

namespace WarehouseApi.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;

    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
    }
}