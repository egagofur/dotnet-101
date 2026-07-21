using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories.Interface;

public interface IProductRepository
{
    Task<IEnumerable<Products>> GetAllAsync();
    Task<Products?> GetByIdAsync(Guid id);
    Task<Products?> GetBySkuAsync(string sku);
    Task<Products> AddAsync(Products product);
    Task<bool> UpdateAsync(Products product);
    Task<bool> DeleteAsync(Guid id);
}