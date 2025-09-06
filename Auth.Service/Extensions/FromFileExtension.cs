using Auth.Service.DTOs.FilesDto;
using Auth.Service.DTOs.Roles;
using Microsoft.AspNetCore.Http;

namespace Auth.Service.Extensions;

public static class FromFileExtension
{
    public static FileForCreationDto ToAttachmentOrDefault(this IFormFile formFile)
    {

        if (formFile?.Length > 0)
        {
            using var ms = new MemoryStream();
            formFile.CopyTo(ms);
            var fileBytes = ms.ToArray();

            return new FileForCreationDto()
            {
                Stream = new MemoryStream(fileBytes)
            };
        }

        return null;
    }
}
