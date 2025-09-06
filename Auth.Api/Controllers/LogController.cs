using Auth.Domain.Configurations;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery] PaginationParams @params,
            [FromQuery] string? search
        )
        {

            // Call the service to get the logs with the necessary filters
            var logs = await _logService.GetAllAsync(
                @params
            );

            // Return the paginated logs as a response
            return Ok(logs);
        }
    }
}
