namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseCommentDto
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }

    public string CommentText { get; set; }
    public int Rating { get; set; } // 1 to 5
}
