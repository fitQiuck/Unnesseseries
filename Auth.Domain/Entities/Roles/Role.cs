using Auth.Domain.Common;
using Auth.Domain.Entities.Permissions;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Roles;

public class Role : Auditable
{
    public string Name { get; set; } = null!; // Admin, Student, Teacher

    public string? Description { get; set; }

    public ICollection<User>? UserRole { get; set; } = new List<User>();

    public ICollection<Permission>? RolePermission { get; set; } = new List<Permission>();
}
