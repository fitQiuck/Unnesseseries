using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Homeworks;

public class UserHomework : Auditable
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid HomeworkId { get; set; }
    public Homework Homework { get; set; } = default!;
    public long Score { get; set; }
    public bool IsCompleted { get; set; }
}
