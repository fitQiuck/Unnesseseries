namespace Auth.Service.Helpers;

public class TokenValidationResult
{
    public bool IsValid { get; set; }
    public bool IsExpired { get; set; }
    public string Message { get; set; } = string.Empty;
}
