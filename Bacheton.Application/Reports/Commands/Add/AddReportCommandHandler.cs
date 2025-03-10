using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Add;

public class AddReportCommandHandler : IRequestHandler<AddReportCommand, ErrorOr<Created>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IImageService _imageService;
    private readonly IAuthUtilities _authUtilities;

    public AddReportCommandHandler(IReportRepository reportRepository, IImageService imageService,
        IAuthUtilities authUtilities)
    {
        _reportRepository = reportRepository;
        _imageService = imageService;
        _authUtilities = authUtilities;
    }

    public async Task<ErrorOr<Created>> Handle(AddReportCommand request, CancellationToken cancellationToken)
    {
        var userId = _authUtilities.GetUserId();
        
        var report = new Report
        {
            Comment = request.Comment,
            Location = request.Location,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Severity = request.Severity,
            UserId = userId
        };

        var imageResult = await _imageService.UploadAsync(fileName: request.Photo.FileName,
            stream: request.Photo.OpenReadStream(), subfolder:$"{userId.ToString()}/reports", cancellationToken: cancellationToken);
        
        if (imageResult.IsError)
            return imageResult.Errors;
        
        report.ImageUrl = imageResult.Value;

        await _reportRepository.InsertAsync(report);

        return Result.Created;
    }
}