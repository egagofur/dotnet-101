using WarehouseApi.DTOs;

namespace WarehouseApi.Services.Interface;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
