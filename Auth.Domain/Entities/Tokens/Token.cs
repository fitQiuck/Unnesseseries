using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Tokens;

public class Token : Auditable
{
    public string AccessToken { get; set; }
    public string? ResetToken { get; set; }
    public Guid UsersId { get; set; }
    public User Users { get; set; }
}
