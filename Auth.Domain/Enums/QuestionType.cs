namespace Auth.Domain.Enums;

public enum QuestionType
{
    MultipleChoice = 1,       // you may render options in UI; backend only needs CorrectAnswer
    TrueFalseNotGiven = 2,
    FillInTheBlank = 3,
    ShortAnswer = 4,
    Matching = 5,
    SentenceCompletion = 6,
    DiagramLabeling = 7
}
