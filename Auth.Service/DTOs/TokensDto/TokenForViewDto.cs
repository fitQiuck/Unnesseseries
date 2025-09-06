using Auth.Domain.Entities.UserManagement;

namespace Auth.Service.DTOs.TokensDto;

public class TokenForViewDto
{
    public Guid Key { get; set; }
    public string AccessToken { get; set; }
    public string? ResetToken { get; set; }
    public Guid UsersId { get; set; }
    public User Users { get; set; }
    public DateTime CreatedAt { get; set; }
}
