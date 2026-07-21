using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task<IEnumerable<Products>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Products?> GetBySkuAsync(string sku)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(x => x.Sku == sku);
    }

    public async Task<Products> AddAsync(Products product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(Products product)
    {
        _context.Products.Update(product);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        return await _context.SaveChangesAsync() > 0;
    }
}