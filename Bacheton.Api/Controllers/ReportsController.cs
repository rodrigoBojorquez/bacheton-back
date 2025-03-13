using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Application.Reports.Commands.Add;
using Bacheton.Application.Reports.Commands.Resolve;
using Bacheton.Application.Reports.Commands.Update;
using Bacheton.Application.Reports.Queries.Find;
using Bacheton.Application.Reports.Queries.List;
using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Common.Adapters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class ReportsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IAuthUtilities _authUtilities;
    private readonly IReportRepository _reportRepository;


    public ReportsController(IMediator mediator, IAuthUtilities authUtilities, IReportRepository reportRepository)
    {
        _mediator = mediator;
        _authUtilities = authUtilities;
        _reportRepository = reportRepository;
    }

    public record MonitoringReportsRequest(
        int Page = 1,
        int PageSize = 10,
        ReportStatus? ReportStatus = null,
        ReportSeverity? ReportSeverity = null,
        Guid? ResolvedById = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        double? Latitude = null,
        double? Longitude = null,
        double? RadiusKm = null);

    public record ListUserReportsRequest(int Page = 1, int PageSize = 10);

    public record CreateReportRequest(
        string Comment,
        string Location,
        double Latitude,
        double Longitude,
        ReportSeverity Severity);

    public record UpdateReportRequest(Guid Id, string Comment, ReportSeverity Severity);

    [HttpGet("monitoring")]
    [RequiredPermission("monitoring:Reportes")]
    public async Task<IActionResult> Monitoring([FromQuery] MonitoringReportsRequest request)
    {
        var query = new ListReportsQuery(
            request.Page,
            request.PageSize,
            request.ReportStatus,
            request.ReportSeverity,
            request.ResolvedById,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Latitude: request.Latitude,
            Longitude: request.Longitude,
            RadiusKm: request.RadiusKm
        );

        var result = await _mediator.Send(query);

        return result.Match(Ok, Problem);
    }

    [HttpGet("user")]
    [RequiredPermission("read:Reportes")]
    public async Task<IActionResult> ListUserReports([FromQuery] ListUserReportsRequest request)
    {
        var userId = _authUtilities.GetUserId();

        var query = new ListReportsQuery(
            request.Page,
            request.PageSize,
            UserId: userId
        );

        var result = await _mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    [RequiredPermission("read:Reportes")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new FindReportQuery(id);
        var result = await _mediator.Send(query);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [RequiredPermission("create:Reportes")]
    public async Task<IActionResult> Create([FromForm] CreateReportRequest request, IFormFile image)
    {
        var command = new AddReportCommand(
            request.Comment,
            request.Location,
            request.Latitude,
            request.Longitude,
            new FormFileAdapter(image),
            request.Severity
        );

        var result = await _mediator.Send(command);

        return result.Match(r => Ok(r), Problem);
    }

    [HttpPut]
    [RequiredPermission("update:Reportes")]
    public async Task<IActionResult> Update([FromBody] UpdateReportRequest request)
    {
        var command = new UpdateReportCommand(request.Id, request.Comment, request.Severity);
        var result = await _mediator.Send(command);
        
        return result.Match(r => Ok(r), Problem);
    }

    [HttpDelete("{id:guid}")]
    [RequiredPermission("delete:Reportes")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reportRepository.DeleteAsync(id);
        return Ok();
    }

    [HttpPatch("{id:guid}/resolve")]
    [RequiredPermission("resolve:Reportes")]
    public async Task<IActionResult> Resolve(Guid id)
    {
        var command = new ResolveReportCommand(id);
        var result = await _mediator.Send(command);

        return result.Match(r => Ok(r), Problem);
    }
}