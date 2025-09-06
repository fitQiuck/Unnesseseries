using Auth.Domain.Common;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Tests;

namespace Auth.Domain.Entities.UserManagement;

public class User : Auditable
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public int PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public int Points { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public ICollection<UserCourse>? UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<UserHomework>? UserHomeworks { get; set; } = new List<UserHomework>();
    public ICollection<TestResult>? TestResult { get; set; } = new List<TestResult>();
    public ICollection<CourseComment>? CourseComments { get; set; } = new List<CourseComment>();

}
