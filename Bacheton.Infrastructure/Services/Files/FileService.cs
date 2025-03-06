using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Errors;
using ErrorOr;

namespace Bacheton.Infrastructure.Services.Files;

public class FileService : IFileService
{
    private readonly string _basePath;

    public FileService()
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }
    
    public async Task<string> UploadFileAsync(Stream stream, string folderName, string fileName)
    {
        string folderPath = Path.Combine(_basePath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string newName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        string filePath = Path.Combine(folderPath, newName);
        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await stream.CopyToAsync(fileStream);
        }
        
        return Path.Combine(folderName, fileName).Replace("\\", "/");
    }

    public void DeleteFileAsync(string path)
    {
        string fullPath = Path.Combine(_basePath, path);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public async Task<ErrorOr<byte[]>> GetFileAsync(string path)
    {
        string fullPath = Path.Combine(_basePath, path);

        if (File.Exists(fullPath))
            return await File.ReadAllBytesAsync(fullPath);
        
        return Errors.Asset.NotFound;
    }
}