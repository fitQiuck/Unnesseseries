using Auth.Domain.Common;
using Auth.Domain.Entities.Roles;

namespace Auth.Domain.Entities.Permissions;

public class Permission : Auditable
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Role> RolePermission { get; set; } = new List<Role>();
}
