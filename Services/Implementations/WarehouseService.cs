using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseApi.DTOs;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Services.Implementations;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<IEnumerable<WarehouseResponse>> GetAllAsync()
    {
        var warehouses = await _warehouseRepository.GetAllAsync();
        return warehouses.Select(MapToResponse);
    }

    public async Task<WarehouseResponse> GetByIdAsync(Guid id)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(id);
        if (warehouse == null)
        {
            throw new KeyNotFoundException($"Gudang dengan ID '{id}' tidak ditemukan.");
        }
        return MapToResponse(warehouse);
    }

    public async Task<WarehouseResponse> CreateAsync(CreateWarehouseRequest request)
    {
        var existing = await _warehouseRepository.GetByCodeAsync(request.Code);
        if (existing != null)
        {
            throw new InvalidOperationException($"Gudang dengan kode '{request.Code}' sudah terdaftar.");
        }

        var warehouse = new Warehouses
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _warehouseRepository.AddAsync(warehouse);
        return MapToResponse(created);
    }

    public async Task<WarehouseResponse> UpdateAsync(Guid id, UpdateWarehouseRequest request)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(id);
        if (warehouse == null)
        {
            throw new KeyNotFoundException($"Gudang dengan ID '{id}' tidak ditemukan.");
        }

        warehouse.Name = request.Name;
        warehouse.Address = request.Address;
        warehouse.City = request.City;
        warehouse.IsActive = request.IsActive;
        warehouse.UpdatedAt = DateTime.UtcNow;

        var success = await _warehouseRepository.UpdateAsync(warehouse);
        if (!success)
        {
            throw new Exception("Gagal memperbarui data gudang.");
        }

        return MapToResponse(warehouse);
    }

    public async Task DeleteAsync(Guid id)
    {
        var success = await _warehouseRepository.DeleteAsync(id);
        if (!success)
        {
            throw new KeyNotFoundException($"Gudang dengan ID '{id}' tidak ditemukan.");
        }
    }

    public async Task<LocationResponse> AddLocationAsync(Guid warehouseId, CreateLocationRequest request)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
        if (warehouse == null)
        {
            throw new KeyNotFoundException($"Gudang dengan ID '{warehouseId}' tidak ditemukan.");
        }

        // Validate location code uniqueness in the warehouse
        if (warehouse.Locations.Any(l => string.Equals(l.Code, request.Code, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Lokasi rak dengan kode '{request.Code}' sudah ada di gudang ini.");
        }

        var location = new WarehouseLocations
        {
            Id = Guid.NewGuid(),
            WarehouseId = warehouseId,
            Code = request.Code,
            Zone = request.Zone,
            Rack = request.Rack,
            Bin = request.Bin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _warehouseRepository.AddLocationAsync(location);
        return MapToLocationResponse(created);
    }

    private static WarehouseResponse MapToResponse(Warehouses warehouse)
    {
        return new WarehouseResponse
        {
            Id = warehouse.Id,
            Code = warehouse.Code,
            Name = warehouse.Name,
            Address = warehouse.Address,
            City = warehouse.City,
            IsActive = warehouse.IsActive,
            CreatedAt = warehouse.CreatedAt,
            UpdatedAt = warehouse.UpdatedAt,
            Locations = warehouse.Locations.Select(MapToLocationResponse).ToList()
        };
    }

    private static LocationResponse MapToLocationResponse(WarehouseLocations location)
    {
        return new LocationResponse
        {
            Id = location.Id,
            Code = location.Code,
            Zone = location.Zone,
            Rack = location.Rack,
            Bin = location.Bin,
            IsActive = location.IsActive
        };
    }
}
