using Auth.Domain.Entities.Messages;
using Auth.Service.DTOs.Users;

namespace Auth.Service.Interfaces;

public interface IEmailService
{
    Task SendMessage(Message Message);
    Task<bool> VerifyEmailAsync(string email, string lenguage);
    Task<bool> GetCodeAsync(string email, string code);
    Task<bool> VerifyAndChangePasswordAsync(UserPasswordForUpdateDto userPasswordForUpdateDto);
}
