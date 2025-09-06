namespace Auth.Service.DTOs.Users;

public class UserPasswordForUpdateDto
{
    public string VerificationCode { get; set; }
    public string Password { get; set; }
}
