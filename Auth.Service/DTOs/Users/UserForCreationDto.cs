namespace Auth.Service.DTOs.Users;

public class UserForCreationDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public Guid RoleId { get; set; }
}
