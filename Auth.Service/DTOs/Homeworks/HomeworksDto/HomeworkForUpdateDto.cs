using Microsoft.AspNetCore.Http;

namespace Auth.Service.DTOs.Homeworks.HomeworksDto;

public class HomeworkForUpdateDto
{
    public IFormFile? Image { get; set; }       
    public string? Definition { get; set; }  
    public string? Answer { get; set; }
}
