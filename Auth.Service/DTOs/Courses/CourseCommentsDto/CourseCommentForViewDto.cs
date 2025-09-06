namespace Auth.Service.DTOs.Courses.CourseCommentsDto;

public class CourseCommentForViewDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
