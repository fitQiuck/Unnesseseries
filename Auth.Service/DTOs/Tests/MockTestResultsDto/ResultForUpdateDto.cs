namespace Auth.Service.DTOs.Tests.MockTestResultsDto;

public class ResultForUpdateDto
{
    public int? ListeningScore { get; set; }
    public int? ReadingScore { get; set; }
    public int? OverallScore { get; set; }     // if null, recompute on save

    public TimeSpan? TimeTaken { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string AdminNote { get; set; }
}
