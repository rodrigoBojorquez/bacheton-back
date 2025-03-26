using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Errors;
using ErrorOr;
using Microsoft.AspNetCore.StaticFiles;

namespace Bacheton.Infrastructure.Services.Assets;

public class ImageService : IImageService
{
    private readonly string _rootPath;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;
    private const long MaxSize = 20 * 1024 * 1024; // 5MB

    public ImageService()
    {
        _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        _contentTypeProvider = new FileExtensionContentTypeProvider();
    }

    public async Task<ErrorOr<string>> UploadAsync(string fileName, Stream stream, string? subfolder,
        CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];
    
        if (stream.Length > MaxSize)
            errors.Add(Errors.Asset.InvalidSize);
    
        if (errors.Any())
            return errors;
    
        var filePath = Path.Combine(_rootPath, subfolder ?? string.Empty, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream, cancellationToken);
    
        // Ruta relativa
        var relativePath = Path.Combine("images", subfolder ?? string.Empty, fileName).Replace("\\", "/");
        return relativePath;
    }

    public Stream? Get(string fileName)
    {
        var filePath = Path.Combine(_rootPath, fileName);

        if (!File.Exists(filePath))
            return null;

        return File.OpenRead(filePath);
    }

    public void Delete(string fileName)
    {
        var filePath = Path.Combine(_rootPath, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}