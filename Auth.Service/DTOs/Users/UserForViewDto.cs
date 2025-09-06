namespace Auth.Service.DTOs.Users;

public class UserForViewDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}
