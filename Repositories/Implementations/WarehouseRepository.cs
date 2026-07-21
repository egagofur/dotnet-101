using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Data;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;

namespace WarehouseApi.Repositories.Implementations;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Warehouses>> GetAllAsync()
    {
        return await _context.Warehouses
            .Include(w => w.Locations)
            .ToListAsync();
    }

    public async Task<Warehouses?> GetByIdAsync(Guid id)
    {
        return await _context.Warehouses
            .Include(w => w.Locations)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Warehouses?> GetByCodeAsync(string code)
    {
        return await _context.Warehouses
            .Include(w => w.Locations)
            .FirstOrDefaultAsync(w => w.Code == code);
    }

    public async Task<Warehouses> AddAsync(Warehouses warehouse)
    {
        await _context.Warehouses.AddAsync(warehouse);
        await _context.SaveChangesAsync();
        return warehouse;
    }

    public async Task<bool> UpdateAsync(Warehouses warehouse)
    {
        _context.Warehouses.Update(warehouse);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null) return false;

        _context.Warehouses.Remove(warehouse);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<WarehouseLocations> AddLocationAsync(WarehouseLocations location)
    {
        await _context.WarehouseLocations.AddAsync(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<WarehouseLocations?> GetLocationByIdAsync(Guid locationId)
    {
        return await _context.WarehouseLocations
            .FirstOrDefaultAsync(loc => loc.Id == locationId);
    }
}
