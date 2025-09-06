using Auth.Domain.Common;
using Auth.Domain.Enums;

namespace Auth.Domain.Entities.Tests;

public class MockTestQuestion : Auditable
{
    public Guid MockTestId { get; set; }
    public MockTest MockTest { get; set; }

    public TestSection TestSection { get; set; }
    public QuestionType QuestionType { get; set; }
    public string QuestionText { get; set; }

    // Optional fields depending on question type
    public string AudioUrl { get; set; }   // for listening
    public string ImageUrl { get; set; }   // for image-based
    public string PassageText { get; set; } // for reading
}
