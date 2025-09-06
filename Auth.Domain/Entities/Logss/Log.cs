using Auth.Domain.Common;

namespace Auth.Domain.Entities.Logs;

public class Log : Auditable
{
    public string? Action { get; set; }           // ex: "Create Bot", "Login", "Delete User"
    public string? TableName { get; set; }        // ex: "Bots", "Users"
    public string? PerformedBy { get; set; }      // ex: username or userId
    public string? Description { get; set; }      // ex: what was changed or added
    public string? IpAddress { get; set; }        // optional
    public string? Method { get; set; }           // GET, POST, PUT, DELETE
}
