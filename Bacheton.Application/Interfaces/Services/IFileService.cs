using ErrorOr;

namespace Bacheton.Application.Interfaces.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream stream, string folderName, string fileName);
    void DeleteFileAsync(string path);
    Task<ErrorOr<byte[]>> GetFileAsync(string path);
}