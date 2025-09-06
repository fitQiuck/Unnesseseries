using Auth.Service.DTOs.Logins;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this._authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authorization (Login)
        /// </summary>
        [HttpPost("login")]
        public async ValueTask<IActionResult> Login(LoginDto dto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

            try
            {
                //error
                var token = await _authService.GenerateToken(dto.Email, dto.Password);

                if (token is null)
                {
                    _logger.LogWarning("Login failed for email: {Email}", dto.Email);
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                _logger.LogInformation("Login successful for email: {Email}", dto.Email);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for email: {Email}", dto.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        [HttpPost("refreshToken")]
        public async ValueTask<IActionResult> RestartToken([FromForm] string refreshToken)
        {
            _logger.LogInformation("Refreshing token...");

            try
            {
                var token = await _authService.RestartToken(refreshToken);
                if (token is null)
                {
                    _logger.LogWarning("Refresh token invalid or expired.");
                    return Unauthorized(new { message = "Invalid refresh token." });
                }

                _logger.LogInformation("Token refreshed successfully.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing token.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get permissions associated with access token
        /// </summary>
        [HttpGet("GetPermissionWithToken")]
        public async ValueTask<IActionResult> GetPermissionWithToken()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                _logger.LogWarning("Authorization header missing.");
                return Unauthorized(new { message = "Authorization header is missing." });
            }

            var bearerToken = authorizationHeader.ToString();
            if (!bearerToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Invalid bearer token format.");
                return Unauthorized(new { message = "Invalid authorization header format." });
            }

            var accessToken = bearerToken.Substring("Bearer ".Length).Trim();

            var token = await _authService.GetPermissinWithToken(accessToken);
            if (token == null)
            {
                _logger.LogWarning("Permission lookup failed. Invalid or expired token.");
                return Unauthorized(new { message = "Invalid or expired token." });
            }

            _logger.LogInformation("Permissions retrieved successfully for token.");
            return Ok(token);
        }
    }
}
