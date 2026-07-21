using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.DTOs;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;
using WarehouseApi.Services.Interface;

namespace WarehouseApi.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly AppDbContext _context;

    public ProductService(IProductRepository productRepository, AppDbContext context)
    {
        _productRepository = productRepository;
        _context = context;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToResponse);
    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<ProductResponse> GetByIdResponseAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Produk dengan ID '{id}' tidak ditemukan.");
        }
        return MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        // Validate Category
        var category = await _context.ProductCategories.FindAsync(request.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Kategori dengan ID '{request.CategoryId}' tidak ditemukan.");
        }

        // Check unique SKU
        var existingProduct = await _productRepository.GetBySkuAsync(request.Sku);
        if (existingProduct != null)
        {
            throw new InvalidOperationException($"Produk dengan SKU '{request.Sku}' sudah terdaftar.");
        }

        var product = new Products
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            Sku = request.Sku,
            Name = request.Name,
            Unit = request.Unit,
            Weight = request.Weight,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdProduct = await _productRepository.AddAsync(product);
        createdProduct.Category = category;

        return MapToResponse(createdProduct);
    }

    public async Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Produk dengan ID '{id}' tidak ditemukan.");
        }

        // Validate Category
        var category = await _context.ProductCategories.FindAsync(request.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Kategori dengan ID '{request.CategoryId}' tidak ditemukan.");
        }

        product.CategoryId = request.CategoryId;
        product.Name = request.Name;
        product.Unit = request.Unit;
        product.Weight = request.Weight;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        var success = await _productRepository.UpdateAsync(product);
        if (!success)
        {
            throw new Exception("Gagal memperbarui data produk.");
        }

        product.Category = category;
        return MapToResponse(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var success = await _productRepository.DeleteAsync(id);
        if (!success)
        {
            throw new KeyNotFoundException($"Produk dengan ID '{id}' tidak ditemukan.");
        }
    }

    private static ProductResponse MapToResponse(Products product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            Sku = product.Sku,
            Name = product.Name,
            Unit = product.Unit,
            Weight = product.Weight,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}