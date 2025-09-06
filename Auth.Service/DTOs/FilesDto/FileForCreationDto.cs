using System.ComponentModel.DataAnnotations;

namespace Auth.Service.DTOs.FilesDto;

public class FileForCreationDto
{
    [Required(ErrorMessage = "File stream is required.")]
    public Stream Stream { get; set; }
}
