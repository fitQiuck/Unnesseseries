using Auth.DataAccess.Interface;
using Auth.Domain.Configurations;
using Auth.Domain.Entities.Logs;
using Auth.Service.DTOs.LogsDto;
using Auth.Service.Extensions;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class LogService : ILogService
{
    private readonly IGenericRepository<Log> _logRepository;
    private readonly IMapper _mapper;

    public LogService(IGenericRepository<Log> _logRepository,
        IMapper mapper)
    {
        this._logRepository = _logRepository;   
        this._mapper = mapper;
    }

    public async Task<PagedResult<LogDto>> GetAllAsync(PaginationParams @params, Expression<Func<Log, bool>> filter = null, string[] includes = null)
    {
        var query = _logRepository.GetAll(filter, includes);
        var pagedEntities = await query.ToPagedListAsync(@params); // Custom extension method
        var dtoItems = _mapper.Map<List<LogDto>>(pagedEntities.Data);

        return new PagedResult<LogDto>
        {
            Data = dtoItems,
            TotalItems = pagedEntities.TotalItems,
            TotalPages = pagedEntities.TotalPages,
            CurrentPage = pagedEntities.CurrentPage,
            PageSize = pagedEntities.PageSize
        };
    }

    public async Task<LogDto> CreateAsync(LogDto dto)
    {
        var entity = _mapper.Map<Log>(dto);
        await _logRepository.CreateAsync(entity);
        await _logRepository.SaveChangesAsync();
        return _mapper.Map<LogDto>(entity);
    }

    public async Task LogAsync(string action, string tableName, string performedBy, string description, string method, string ipAddress = null)
    {
        var log = new Log
        {
            Action = action,
            TableName = tableName,
            PerformedBy = performedBy,
            Description = description,
            Method = method,
            IpAddress = ipAddress,
        };

        await _logRepository.CreateAsync(log);
        await _logRepository.SaveChangesAsync();
    }

    public async Task ErrorLogAsync(Exception ex, string path, string method, string performedBy = "System", string ipAddress = null)
    {
        var log = new Log
        {
            Action = "Error",
            TableName = null,
            PerformedBy = performedBy,
            Description = ex.Message,
            Method = method,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        await _logRepository.CreateAsync(log);
        await _logRepository.SaveChangesAsync();
    }
}
