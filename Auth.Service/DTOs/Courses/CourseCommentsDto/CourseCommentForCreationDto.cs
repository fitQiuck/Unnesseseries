namespace Auth.Service.DTOs.Courses.CourseCommentsDto;

public class CourseCommentForCreationDto
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}
