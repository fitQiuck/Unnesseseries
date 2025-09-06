namespace Auth.Service.DTOs.Tests.MockTestResultsDto;

public class MockTestSubmissionDto
{
    public Guid MockTestId { get; set; }
    public Guid UserId { get; set; }           // or take from auth context and drop this

    // key = QuestionId, value = student's answer (string)
    public Dictionary<Guid, string> Answers { get; set; }

    public TimeSpan TimeTaken { get; set; }
}
