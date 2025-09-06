using Auth.Domain.Configurations;
using Auth.Domain.Entities.Logs;
using Auth.Service.DTOs.LogsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ILogService
{
    Task LogAsync(
        string action,
        string tableName,
        string performedBy,
        string description,
        string method,
        string ipAddress = null
    );
    Task ErrorLogAsync(Exception ex, string path, string method, string performedBy = "System", string ipAddress = null);

    Task<PagedResult<LogDto>> GetAllAsync(
        PaginationParams @params,
        Expression<Func<Log, bool>> filter = null,
        string[] includes = null
    );

    Task<LogDto> CreateAsync(LogDto dto);
}
