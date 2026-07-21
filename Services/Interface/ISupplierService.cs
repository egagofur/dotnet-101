using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.DTOs;

namespace WarehouseApi.Services.Interface;

public interface ISupplierService
{
    Task<IEnumerable<SupplierResponse>> GetAllAsync();
    Task<SupplierResponse> GetByIdAsync(Guid id);
    Task<SupplierResponse> CreateAsync(CreateSupplierRequest request);
    Task<SupplierResponse> UpdateAsync(Guid id, UpdateSupplierRequest request);
    Task DeleteAsync(Guid id);
}
