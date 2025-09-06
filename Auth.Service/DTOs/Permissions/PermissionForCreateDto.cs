namespace Auth.Service.DTOs.Permissions;

public class PermissionForCreateDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
}
