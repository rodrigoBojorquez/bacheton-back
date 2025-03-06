using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Errors;
using ErrorOr;
using Microsoft.AspNetCore.StaticFiles;

namespace Bacheton.Infrastructure.Services.Assets;

public class ImageService : IImageService
{
    private readonly string _rootPath;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;
    private readonly long _maxSize = 5 * 1024 * 1024; // 5MB

    public ImageService()
    {
        _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images");
        _contentTypeProvider = new FileExtensionContentTypeProvider();
    }

    public async Task<ErrorOr<string>> UploadAsync(string fileName, Stream stream, string? subfolder,
        CancellationToken cancellationToken = default)
    {
        List<Error> errors = new();

        if (!_contentTypeProvider.TryGetContentType(fileName, out var contentType) || !contentType.StartsWith("image/"))
            errors.Add(Errors.Asset.InvalidContentType);
    
        if (stream.Length > _maxSize)
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