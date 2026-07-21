using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApi.Models;

namespace WarehouseApi.Repositories.Interface;

public interface IStockMovementRepository
{
    Task<IEnumerable<StockMovements>> GetAllAsync();
    Task<StockMovements?> GetByIdAsync(Guid id);
    Task<StockMovements> AddAsync(StockMovements movement);
}
