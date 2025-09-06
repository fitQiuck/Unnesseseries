using Auth.Domain.Common;

namespace Auth.Domain.Entities.ErrorsDetails;

public class ErrorDetails : Auditable
{
    public DateTime Timestamp { get; set; }
    public int StatusCode { get; set; }
    public string Path { get; set; }
    public string Method { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
    public string ExceptionType { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string InnerException { get; set; }
}
