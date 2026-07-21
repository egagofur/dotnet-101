using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.DTOs;
using WarehouseApi.Models;

namespace WarehouseApi.Services.Interface;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<Products?> GetByIdAsync(Guid id);
    Task<ProductResponse> GetByIdResponseAsync(Guid id);
    Task<ProductResponse> CreateAsync(CreateProductRequest request);
    Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request);
    Task DeleteAsync(Guid id);
}