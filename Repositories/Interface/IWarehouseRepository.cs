using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories.Interface;

public interface IWarehouseRepository
{
    Task<IEnumerable<Warehouses>> GetAllAsync();
    Task<Warehouses?> GetByIdAsync(Guid id);
    Task<Warehouses?> GetByCodeAsync(string code);
    Task<Warehouses> AddAsync(Warehouses warehouse);
    Task<bool> UpdateAsync(Warehouses warehouse);
    Task<bool> DeleteAsync(Guid id);
    Task<WarehouseLocations> AddLocationAsync(WarehouseLocations location);
    Task<WarehouseLocations?> GetLocationByIdAsync(Guid locationId);
}
