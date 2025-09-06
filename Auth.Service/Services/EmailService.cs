using Auth.Domain.Entities.Messages;
using Auth.Service.DTOs.Users;
using Auth.Service.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MimeKit;
using QRCoder;
using MailKit.Net.Smtp;

namespace Auth.Service.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IMemoryCache memoryCache;

    public EmailService(IConfiguration configuration,
        IMemoryCache memoryCache,
        IUserService userService)
    {
        _configuration = configuration;
        this.memoryCache = memoryCache;
        _userService = userService;
    }

    public async Task<bool> GetCodeAsync(string email, string code)
    {
        var cashedValus = memoryCache.Get<string>(email);
        if (cashedValus == code)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> VerifyAndChangePasswordAsync(UserPasswordForUpdateDto userPasswordForUpdateDto)
    {
        if (!memoryCache.TryGetValue(userPasswordForUpdateDto.VerificationCode, out string cachedEmail))
        {
            throw new KeyNotFoundException("Verification code is invalid or expired.");
        }

        var isPasswordChanged = await _userService.ChangePassword(cachedEmail, userPasswordForUpdateDto.Password);

        if (isPasswordChanged)
        {
            memoryCache.Remove(userPasswordForUpdateDto.VerificationCode);
            return true;
        }

        throw new Exception("Failed to change password. Please try again later.");
    }

    public async Task SendMessage(Message message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["EmailAddress"]));
        email.To.Add(MailboxAddress.Parse(message.To));

        email.Subject = message.Subject;
        email.Body = new TextPart("html")
        {
            Text = message.Body
        };

        using var smtp = new SmtpClient();
        smtp.Connect(_configuration["Host"], 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(_configuration["EmailAddress"], _configuration["Password"]);
        smtp.Send(email);
        smtp.Disconnect(true);
    }

    public async Task<bool> VerifyEmailAsync(string email, string lenguage)
    {
        var res = await this._userService.GetAsync(item => item.Email == email);

        if (res == null)
        {
            throw new Exception($"User with email {email} does not exist.");
        }

        var verificationCode = Guid.NewGuid().ToString();
        var verificationLink = $"{_configuration["Domain"]}{lenguage}/reset-password/{verificationCode}";

        string base64Qr;

        // QR kod yaratish
        using (var qrGenerator = new QRCodeGenerator())
        using (var qrCodeData = qrGenerator.CreateQrCode(verificationLink, QRCodeGenerator.ECCLevel.Q))
        using (var qrCode = new PngByteQRCode(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(5);
            base64Qr = Convert.ToBase64String(qrCodeImage);
        }

        // Email xabari
        var message = new Message()
        {
            Subject = "Do not share this code with others",
            To = email,
            Body = $@"
        <html>
        <body>
            <p>Scan the QR code to verify your email:</p>
            <img src='data:image/png;base64,{base64Qr}' />
            <p>Or click this link: <a href='{verificationLink}'>Verify Email</a></p>
        </body>
        </html>"
        };

        // Cache'ga kodni saqlash
        memoryCache.Set(verificationCode, email, TimeSpan.FromMinutes(30));

        // Emailni yuborish
        await SendMessage(message);

        return true;
    }
}
