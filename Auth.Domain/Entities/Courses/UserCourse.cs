using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Courses;

public class UserCourse : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public int? Rating { get; set; } // from 1 to 5
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
