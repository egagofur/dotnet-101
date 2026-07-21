using WarehouseApi.Data;
using WarehouseApi.DTOs;
using WarehouseApi.Models;
using WarehouseApi.Repositories.Interface;
using WarehouseApi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;

    public UserService(IUserRepository userRepository, AppDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToResponse);
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User dengan ID '{id}' tidak ditemukan.");
        }
        return MapToResponse(user);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' already exists.");
            }

            var now = DateTime.UtcNow;
            var user = new Users
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                IsActive = request.Status,
                CreatedAt = now,
                UpdatedAt = now
            };

            var createdUser = await _userRepository.AddAsync(user);

            await transaction.CommitAsync();
            return MapToResponse(createdUser);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User dengan ID '{id}' tidak ditemukan.");
            }

            // Check if email changed and is taken by another user
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException($"User dengan email '{request.Email}' sudah terdaftar.");
                }
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Role = request.Role;
            user.IsActive = request.Status;
            user.UpdatedAt = DateTime.UtcNow;

            var success = await _userRepository.UpdateAsync(user);
            if (!success)
            {
                throw new Exception("Gagal memperbarui data user.");
            }

            await transaction.CommitAsync();
            return MapToResponse(user);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var success = await _userRepository.DeleteAsync(id);
            if (!success)
            {
                throw new KeyNotFoundException($"User dengan ID '{id}' tidak ditemukan.");
            }
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static UserResponse MapToResponse(Users user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Status = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
