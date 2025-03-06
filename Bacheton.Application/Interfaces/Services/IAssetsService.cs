using ErrorOr;

namespace Bacheton.Application.Interfaces.Services;

public interface IAssetsService
{
    Task<ErrorOr<string>> UploadAsync(string fileName, Stream stream, string? subfolder, CancellationToken cancellationToken = default);
    Stream? Get(string fileName);
    void Delete(string fileName);
}