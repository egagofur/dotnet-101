using WarehouseApi.Enums;

namespace WarehouseApi.Models;

public class Users : BaseModel
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
}
