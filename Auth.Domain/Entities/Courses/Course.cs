using Auth.Domain.Common;
using Auth.Domain.Enums;

namespace Auth.Domain.Entities.Courses;

public class Course : Auditable
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? LevelId { get; set; }
    public CourseLevel? Level { get; set; }
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<CourseComment> Comments { get; set; } = new List<CourseComment>();

}
