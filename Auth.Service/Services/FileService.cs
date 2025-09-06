using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Auth.Service.Services;

public class FileService : IFileService
{
    public async Task<string> UploadAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File cannot be empty");

        // Ensure folder path
        string folderPath = Path.Combine(EnvironmentHelper.WebRootPath, folderName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Generate unique filename
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(folderPath, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative path (e.g., "TestMaterials/abc123.pdf")
        return Path.Combine(folderName, fileName).Replace("\\", "/");
    }

    public async Task<bool> DeleteAsync(string filePath)
    {
        string fullPath = Path.Combine(EnvironmentHelper.WebRootPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }
}
