namespace Auth.Service.DTOs.Roles;

public class RoleForUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public object? Permissions { get; set; }
}
