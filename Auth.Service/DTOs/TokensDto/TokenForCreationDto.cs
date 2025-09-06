namespace Auth.Service.DTOs.TokensDto;

public class TokenForCreationDto
{
    public string AccessToken { get; set; }
    public string? ResetToken { get; set; }
    public Guid UsersId { get; set; }
}
