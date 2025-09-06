using Auth.Domain.Entities.Tokens;
using Auth.Service.DTOs.TokensDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ITokenService
{
    Task<List<TokenForViewDto>> GetAllAsync(Expression<Func<Token, bool>> filter = null, string[] includes = null);
    Task<TokenForViewDto> GetAsync(Expression<Func<Token, bool>> filter = null, string[] includes = null);
    Task<TokenForViewDto> CreateAsync(TokenForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<Token, bool>> filter);
    Task<TokenForViewDto> UpdateAsync(TokenForCreationDto dto);
    Task<bool> CheckTokenExistsAsync(string accessToken);
}
