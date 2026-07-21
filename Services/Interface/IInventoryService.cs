using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.DTOs;

namespace WarehouseApi.Services.Interface;

public interface IInventoryService
{
    Task<IEnumerable<StockMovementResponse>> GetAllMovementsAsync();
    Task<StockMovementResponse> GetMovementByIdAsync(Guid id);
    Task<StockMovementResponse> CreateMovementAsync(Guid userId, string userEmail, StockMovementRequest request);
}
