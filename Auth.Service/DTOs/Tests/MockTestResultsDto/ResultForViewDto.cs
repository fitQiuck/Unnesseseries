namespace Auth.Service.DTOs.Tests.MockTestResultsDto;

public class ResultForViewDto
{
    public Guid Id { get; set; }
    public Guid MockTestId { get; set; }
    public Guid UserId { get; set; }

    public int ListeningScore { get; set; }    // 0..100 (percentage)
    public int ReadingScore { get; set; }      // 0..100
    public int OverallScore { get; set; }      // average of the two, rounded

    public TimeSpan TimeTaken { get; set; }
    public DateTime CompletedAt { get; set; }
}
