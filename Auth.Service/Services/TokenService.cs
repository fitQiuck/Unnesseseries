using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Tokens;
using Auth.Service.DTOs.TokensDto;
using Auth.Service.Exceptions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class TokenService : ITokenService
{
    private readonly IGenericRepository<Token> _tokenRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IGenericRepository<Token> _tokenRepository,
        IMapper _mapper,
        ILogger<TokenService> _logger)
    {
        this._mapper = _mapper;
        this._tokenRepository = _tokenRepository;
        this._logger = _logger;
    }

    public async Task<List<TokenForViewDto>> GetAllAsync(
        Expression<Func<Token, bool>> filter = null,
        string[] includes = null)
    {
        _logger.LogInformation("Fetching all tokens...");

        var query = _tokenRepository.GetAll(filter, includes: new[] { "Users" });
        var result = _mapper.Map<List<TokenForViewDto>>(query)
                            .OrderBy(t => t.CreatedAt)
                            .ToList();

        _logger.LogInformation("{Count} tokens fetched", result.Count);
        return result;
    }

    public async Task<TokenForViewDto> GetAsync(
        Expression<Func<Token, bool>> filter = null,
        string[] includes = null)
    {
        _logger.LogInformation("Fetching token with specified filter...");

        var result = await _tokenRepository.GetAsync(filter, includes: ["Users"]);
        if (result == null)
        {
            _logger.LogWarning("Token not found with given filter");
            throw new HttpStatusCodeException(404, "Token not found");
        }

        _logger.LogInformation("Token found for user: {UserId}", result.UsersId);
        return _mapper.Map<TokenForViewDto>(result);
    }

    public async Task<TokenForViewDto> CreateAsync(TokenForCreationDto dto)
    {
        _logger.LogInformation("Creating token for user: {UserId}", dto.UsersId);

        // Optional: Uncomment to prevent duplicate tokens
        // var existUser = await _tokenRepository.GetAsync(p => p.UsersId == dto.UsersId);
        // if (existUser != null)
        // {
        //     _logger.LogWarning("Token already exists for user: {UserId}", dto.UsersId);
        //     throw new HttpStatusCodeException(400, "Token already exists");
        // }

        var tokenEntity = _mapper.Map<Token>(dto);

        await _tokenRepository.CreateAsync(tokenEntity);
        await _tokenRepository.SaveChangesAsync();

        _logger.LogInformation("Token successfully created for user: {UserId}", dto.UsersId);
        return _mapper.Map<TokenForViewDto>(tokenEntity);
    }

    public Task<bool> DeleteAsync(Expression<Func<Token, bool>> filter)
    {
        _logger.LogWarning("DeleteAsync is not implemented in TokenService");
        throw new NotImplementedException();
    }

    public async Task<TokenForViewDto> UpdateAsync(TokenForCreationDto dto)
    {
        _logger.LogInformation("Updating token for user: {UserId}", dto.UsersId);

        var existingToken = await _tokenRepository.GetAsync(item => item.UsersId == dto.UsersId);
        if (existingToken == null)
        {
            _logger.LogWarning("Token not found for user: {UserId}", dto.UsersId);
            throw new HttpStatusCodeException(404, "Token not found");
        }

        existingToken.AccessToken = dto.AccessToken;
        existingToken.UpdatedAt = DateTime.UtcNow;
        existingToken.UpdatedBy = HttpContextHelper.UserId;

        _tokenRepository.Update(existingToken);
        await _tokenRepository.SaveChangesAsync();

        _logger.LogInformation("Token updated for user: {UserId}", dto.UsersId);
        return _mapper.Map<TokenForViewDto>(existingToken);
    }

    public async Task<bool> CheckTokenExistsAsync(string accessToken)
    {
        var token = await _tokenRepository.GetAsync(t => t.AccessToken == accessToken);
        return token != null;
    }
}
