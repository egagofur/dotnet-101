using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.DTOs;

namespace WarehouseApi.Services.Interface;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseResponse>> GetAllAsync();
    Task<WarehouseResponse> GetByIdAsync(Guid id);
    Task<WarehouseResponse> CreateAsync(CreateWarehouseRequest request);
    Task<WarehouseResponse> UpdateAsync(Guid id, UpdateWarehouseRequest request);
    Task DeleteAsync(Guid id);
    Task<LocationResponse> AddLocationAsync(Guid warehouseId, CreateLocationRequest request);
}
