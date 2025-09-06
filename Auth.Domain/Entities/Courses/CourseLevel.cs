using Auth.Domain.Common;

namespace Auth.Domain.Entities.Courses;

public class CourseLevel : Auditable
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
