using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Tokens;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.TokensDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using Auth.Service.Security;
using Auth.Service.Helpers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using TokenValidationResult = Auth.Service.Helpers.TokenValidationResult;
using Microsoft.Extensions.Logging;

namespace Auth.Service.Services; 

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> userRepository;
    private readonly IGenericRepository<Role> roleRepository;
    private readonly IGenericRepository<Token> tokenRepository;
    private readonly IPermissionService permissionService;
    private readonly ILogger<AuthService> logger;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public AuthService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository,
        IGenericRepository<Token> tokenRepository, IPermissionService permissionService,
        IMapper mapper, IConfiguration configuration, ILogger<AuthService> logger)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.tokenRepository = tokenRepository;
        this.permissionService = permissionService;
        this.mapper = mapper;
        this.logger = logger;
        this.configuration = configuration;
    }

    public async ValueTask<TokenForViewDto> GenerateToken(string Username, string password)
    {
        logger.LogInformation("Generating token for user: {Username}", Username);

        var user = await userRepository.GetAsync(p => p.Email == Username);
        if (user == null)
        {
            logger.LogWarning("Login failed for username: {Username}", Username);
            throw new HttpStatusCodeException(400, "Login or Password is incorrect");
        }

        if (!SecurePasswordHasher.Verify(password, user.PasswordHash))
        {
            logger.LogWarning("Password mismatch for user: {Username}", Username);
            throw new HttpStatusCodeException(400, "Login or Password is incorrect");
        }

        var role = await roleRepository.GetAsync(p => p.Id == user.RoleId, includes: ["RolePermission"]);
        if (role == null)
        {
            logger.LogError("Role not found for user {UserId}", user.Id);
            throw new HttpStatusCodeException(400, "Role not found");
        }

        var permissionNames = role.RolePermission.Select(rp => rp.Name).ToList();
        var jsonPermissions = JsonSerializer.Serialize(permissionNames);

        var jwtExpireStr = configuration["JWT:Expire"];
        var jwtResExpireStr = configuration["JWT:ResExpire"];
        var jwtKey = configuration["JWT:Key"];
        var jwtIssuer = configuration["JWT:ValidIssuer"];

        if (string.IsNullOrWhiteSpace(jwtExpireStr) || string.IsNullOrWhiteSpace(jwtResExpireStr) ||
            string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer))
        {
            logger.LogError("JWT configuration values are missing");
            throw new InvalidOperationException("JWT configuration values are missing");
        }

        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JWT:Key"])
        );

                var authClaims = new List<Claim>
        {
            new Claim("Permissions", jsonPermissions),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, role.Name)
        };

        // ✅ Access Token
        var accessToken = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"], // 👈 add audience
            expires: DateTime.UtcNow.AddMinutes(int.Parse(configuration["JWT:Expire"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        // ✅ Refresh Token
        var refreshToken = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"], // 👈 add audience
            expires: DateTime.UtcNow.AddDays(int.Parse(configuration["JWT:Expire"])),
            claims: new List<Claim>
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );


        var tokenModel = new Token
        {
            UsersId = user.Id,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            ResetToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
            CreatedAt = DateTime.UtcNow
        };

        var existingToken = await tokenRepository.GetAsync(p => p.UsersId == user.Id);
        if (existingToken == null)
        {
            logger.LogInformation("Creating new token for user {UserId}", user.Id);
            await tokenRepository.CreateAsync(tokenModel);
        }
        else
        {
            logger.LogInformation("Updating existing token for user {UserId}", user.Id);
            existingToken.AccessToken = tokenModel.AccessToken;
            existingToken.ResetToken = tokenModel.ResetToken;
            existingToken.CreatedAt = DateTime.UtcNow.AddHours(5);
            tokenRepository.Update(existingToken);
        }

        await tokenRepository.SaveChangesAsync();

        logger.LogInformation("Token successfully generated for user {UserId}", user.Id);

        return mapper.Map<TokenForViewDto>(tokenModel);
    }

    public async ValueTask<string> RestartToken(string token)
    {
        logger.LogInformation("Restarting token");

        if (IsTokenExpired(token))
        {
            logger.LogWarning("Token is expired");
            throw new HttpStatusCodeException(400, "Token is expired");
        }

        var existingToken = await tokenRepository.GetAsync(p => p.ResetToken == token);
        if (existingToken == null)
        {
            logger.LogWarning("Token not found in DB");
            throw new HttpStatusCodeException(400, "Token not found");
        }

        var user = await userRepository.GetAsync(p => p.Id == existingToken.UsersId);
        if (user == null)
        {
            logger.LogWarning("User not found for token");
            throw new HttpStatusCodeException(400, "User not found");
        }

        var role = await roleRepository.GetAsync(p => p.Id == user.RoleId, includes: ["RolePermission"]);
        if (role == null)
        {
            logger.LogError("Role not found while restarting token");
            throw new HttpStatusCodeException(400, "Role not found");
        }

        var permissions = JsonSerializer.Serialize(role.RolePermission.Select(r => r.Name));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        var newAccessToken = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            expires: DateTime.Now.AddDays(int.Parse(configuration["JWT:Expire"])),
            claims: new List<Claim>
            {
                new Claim("Permissions", permissions),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role.Name)
            },
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        existingToken.AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
        existingToken.CreatedAt = DateTime.UtcNow.AddHours(5);

        tokenRepository.Update(existingToken);

        await tokenRepository.SaveChangesAsync();
        logger.LogInformation("Token refreshed successfully for user {UserId}", user.Id);

        return new JwtSecurityTokenHandler().WriteToken(newAccessToken);
    }

    public async ValueTask<Dictionary<string, Dictionary<string, bool>>> GetPermissinWithToken(string token)
    {
        logger.LogInformation("Getting permissions from token");

        if (string.IsNullOrWhiteSpace(token))
        {
            logger.LogWarning("Token is null or empty");
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));
        }

        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogError("UserId not found in token");
                throw new Exception("User ID not found in the token.");
            }

            var user = await userRepository.GetAsync(p => p.Id == Guid.Parse(userId));
            if (user == null)
            {
                logger.LogWarning("User not found for permissions");
                throw new HttpStatusCodeException(400, "User not found");
            }

            var role = await roleRepository.GetAsync(p => p.Id == user.RoleId, includes: ["RolePermission"]);
            if (role == null)
            {
                logger.LogWarning("Role not found while retrieving permissions");
                throw new HttpStatusCodeException(400, "Role not found");
            }

            var permissionNames = role.RolePermission.Select(r => r.Name).ToList();
            var permissions = await permissionService.GetPermissionsAsync(permissionNames);

            logger.LogInformation("Permissions successfully retrieved for user {UserId}", user.Id);
            return permissions;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing token");
            throw;
        }
    }

    private bool IsTokenExpired(string token)
    {
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking token expiration");
            throw;
        }
    }
}
