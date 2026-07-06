using WarehouseApi.Enums;

namespace WarehouseApi.Models;

public class Roles
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public RolesNameEnum Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Users> Users { get; set; } = new List<Users>();
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
