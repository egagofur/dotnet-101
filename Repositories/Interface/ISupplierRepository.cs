using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories.Interface;

public interface ISupplierRepository
{
    Task<IEnumerable<Suppliers>> GetAllAsync();
    Task<Suppliers?> GetByIdAsync(Guid id);
    Task<Suppliers?> GetByCodeAsync(string code);
    Task<Suppliers> AddAsync(Suppliers supplier);
    Task<bool> UpdateAsync(Suppliers supplier);
    Task<bool> DeleteAsync(Guid id);
}
