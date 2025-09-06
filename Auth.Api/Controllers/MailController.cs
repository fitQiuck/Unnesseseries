using Auth.Service.DTOs.Emails;
using Auth.Service.DTOs.Users;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : Controller
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService _emailService)
        {
            _emailService = _emailService;
        }

        [HttpPost("mail")]
        public async Task<IActionResult> SendEmail([FromBody] MailDto mailDto)
        {
            string? userLanguage = Request.Headers["Accept-Language"].ToString();

            userLanguage = string.IsNullOrEmpty(userLanguage) ? "uz" : userLanguage;

            var data = await _emailService.VerifyEmailAsync(mailDto.email, userLanguage);

            return Ok(data);
        }


        [HttpPost("chekAndChange")]
        public async Task<IActionResult> ChekAndChangeEmail([FromBody] UserPasswordForUpdateDto userPasswordForUpdateDto)
        {
            var data = await this._emailService.VerifyAndChangePasswordAsync(userPasswordForUpdateDto);
            return Ok(data);
        }
    }
}
