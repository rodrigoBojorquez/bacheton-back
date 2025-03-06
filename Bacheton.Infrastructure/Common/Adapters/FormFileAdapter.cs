using Bacheton.Application.Common.Files;
using Microsoft.AspNetCore.Http;

namespace Bacheton.Infrastructure.Common.Adapters;

public class FormFileAdapter : IFile
{
    private readonly IFormFile _formFile;
    
    public FormFileAdapter(IFormFile formFile)
    {
        _formFile = formFile;
    }
    
    public string ContentType => _formFile.ContentType;
    public long Length => _formFile.Length;
    public string FileName => _formFile.FileName;
    public string Name => _formFile.Name;
    public Stream OpenReadStream() => _formFile.OpenReadStream();
}