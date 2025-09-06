using Auth.Domain.Common;
using Auth.Domain.Entities.Homeworks;

namespace Auth.Domain.Entities.Courses;

public class Lesson : Auditable
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    // Store a relative path (e.g., "videos/lessons/abcd.mp4") or a full URL if you prefer
    public string VideoPath { get; set; } = string.Empty;

    // Optional: store video length if you want to display it in the UI
    public int? VideoDurationSeconds { get; set; }

    // Lesson ordering inside a course (0 = unspecified)
    public int Order { get; set; } = 0;

    // Relations
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;

    // Reverse nav for your existing Homework entity
    public ICollection<Homework> Homeworks { get; set; } = new List<Homework>();
}
