using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseApi.DTOs;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IEnumerable<SupplierResponse>> GetAllAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return suppliers.Select(MapToResponse);
    }

    public async Task<SupplierResponse> GetByIdAsync(Guid id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Supplier dengan ID '{id}' tidak ditemukan.");
        }
        return MapToResponse(supplier);
    }

    public async Task<SupplierResponse> CreateAsync(CreateSupplierRequest request)
    {
        var existing = await _supplierRepository.GetByCodeAsync(request.Code);
        if (existing != null)
        {
            throw new InvalidOperationException($"Supplier dengan kode '{request.Code}' sudah terdaftar.");
        }

        var supplier = new Suppliers
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _supplierRepository.AddAsync(supplier);
        return MapToResponse(created);
    }

    public async Task<SupplierResponse> UpdateAsync(Guid id, UpdateSupplierRequest request)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Supplier dengan ID '{id}' tidak ditemukan.");
        }

        supplier.Name = request.Name;
        supplier.Email = request.Email;
        supplier.Phone = request.Phone;
        supplier.Address = request.Address;
        supplier.IsActive = request.IsActive;
        supplier.UpdatedAt = DateTime.UtcNow;

        var success = await _supplierRepository.UpdateAsync(supplier);
        if (!success)
        {
            throw new Exception("Gagal memperbarui data supplier.");
        }

        return MapToResponse(supplier);
    }

    public async Task DeleteAsync(Guid id)
    {
        var success = await _supplierRepository.DeleteAsync(id);
        if (!success)
        {
            throw new KeyNotFoundException($"Supplier dengan ID '{id}' tidak ditemukan.");
        }
    }

    private static SupplierResponse MapToResponse(Suppliers supplier)
    {
        return new SupplierResponse
        {
            Id = supplier.Id,
            Code = supplier.Code,
            Name = supplier.Name,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }
}
