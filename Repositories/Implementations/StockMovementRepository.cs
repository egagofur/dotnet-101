using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Data;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;

namespace WarehouseApi.Repositories.Implementations;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _context;

    public StockMovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockMovements>> GetAllAsync()
    {
        return await _context.StockMovements
            .Include(m => m.Supplier)
            .Include(m => m.Creator)
            .Include(m => m.Approver)
            .Include(m => m.Items)
                .ThenInclude(i => i.Product)
            .Include(m => m.Items)
                .ThenInclude(i => i.SourceLocation)
            .Include(m => m.Items)
                .ThenInclude(i => i.DestinationLocation)
            .ToListAsync();
    }

    public async Task<StockMovements?> GetByIdAsync(Guid id)
    {
        return await _context.StockMovements
            .Include(m => m.Supplier)
            .Include(m => m.Creator)
            .Include(m => m.Approver)
            .Include(m => m.Items)
                .ThenInclude(i => i.Product)
            .Include(m => m.Items)
                .ThenInclude(i => i.SourceLocation)
            .Include(m => m.Items)
                .ThenInclude(i => i.DestinationLocation)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<StockMovements> AddAsync(StockMovements movement)
    {
        await _context.StockMovements.AddAsync(movement);
        await _context.SaveChangesAsync();
        return movement;
    }
}
