namespace Auth.Service.DTOs.Roles;

public class RoleForCreationDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public object? Permissions { get; set; }
    public ICollection<Guid>? RolePermissions { get; set; }
}
