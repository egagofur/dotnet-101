namespace WarehouseApi.DTOs;

public class LoginResponse
{
    public required string Token { get; set; }
    public required UserResponse User { get; set; }
}
