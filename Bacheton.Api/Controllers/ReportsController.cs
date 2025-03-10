using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Application.Reports.Queries.List;
using Bacheton.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class ReportsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IAuthUtilities _authUtilities;

    public ReportsController(IMediator mediator, IAuthUtilities authUtilities)
    {
        _mediator = mediator;
        _authUtilities = authUtilities;
    }

    public record MonitoringReportsRequest(
        int Page,
        int PageSize,
        ReportStatus? ReportStatus = null,
        ReportSeverity? ReportSeverity = null,
        Guid? ResolvedById = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        double? Latitude = null,
        double? Longitude = null,
        double? RadiusKm = null);

    public record ListUserReportsRequest(int Page, int PageSize);

    public record CreateReportRequest(
        string Comment,
        string Location,
        double Latitude,
        double Longitude,
        ReportSeverity Severity,
        IFormFile image);
    public record UpdateReportRequest(string Comment, ReportSeverity Severity);

    [HttpGet]
    [RequiredPermission("monitoring:Reportes")]
    public async Task<IActionResult> Monitoring([FromQuery] MonitoringReportsRequest request)
    {
        var query = new ListReportsQuery(
            request.Page,
            request.PageSize,
            request.ReportStatus,
            request.ReportSeverity,
            request.ResolvedById,
            StartDate:request.StartDate,
            EndDate:request.EndDate,
            Latitude:request.Latitude,
            Longitude:request.Longitude,
            RadiusKm:request.RadiusKm
        );
        
        var result = await _mediator.Send(query);

        return result.Match(Ok, Problem);
    }

    [HttpGet]
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

    [HttpGet("{id}")]
    [RequiredPermission("read:Reportes")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    [RequiredPermission("create:Reportes")]
    public async Task<IActionResult> Create([FromForm] CreateReportRequest request)
    {
        return Ok();
    }

    [HttpPut]
    [RequiredPermission("update:Reportes")]
    public async Task<IActionResult> Update([FromBody] UpdateReportRequest request)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    [RequiredPermission("delete:Reportes")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok();
    }
    
    [HttpPatch("{id}/resolve")]
    [RequiredPermission("resolve:Reportes")]
    public async Task<IActionResult> Resolve(Guid id)
    {
        return Ok();
    }
}