using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Courses;

public class CourseComment : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } // optional

}
