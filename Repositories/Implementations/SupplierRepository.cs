using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Data;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;

namespace WarehouseApi.Repositories.Implementations;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Suppliers>> GetAllAsync()
    {
        return await _context.Suppliers.ToListAsync();
    }

    public async Task<Suppliers?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Suppliers?> GetByCodeAsync(string code)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(s => s.Code == code);
    }

    public async Task<Suppliers> AddAsync(Suppliers supplier)
    {
        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<bool> UpdateAsync(Suppliers supplier)
    {
        _context.Suppliers.Update(supplier);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        _context.Suppliers.Remove(supplier);
        return await _context.SaveChangesAsync() > 0;
    }
}
