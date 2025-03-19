using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Reports.Queries.AverageResolutionTime;
using Bacheton.Application.Reports.Queries.SeverityCount;
using Bacheton.Application.Reports.Queries.Summary;
using Bacheton.Application.User.Queries.TopUsers;
using Bacheton.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class DashboardController(IMediator mediator) : ApiController
{
    [RequiredPermission(BachetonConstants.Permissions.SuperAccessPermission)]
    [HttpGet("reports-summary")]
    public async Task<IActionResult> GetReportsSummary()
    {
        var query = new ReportsSummaryQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [RequiredPermission(BachetonConstants.Permissions.SuperAccessPermission)]
    [HttpGet("reports-severity")]
    public async Task<IActionResult> GetReportsSeverity()
    {
        var query = new SeverityCountQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [RequiredPermission(BachetonConstants.Permissions.SuperAccessPermission)]
    [HttpGet("top-users")]
    public async Task<IActionResult> GetTopUsers()
    {
        var query = new TopUsersQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [RequiredPermission(BachetonConstants.Permissions.SuperAccessPermission)]
    [HttpGet("average-resolution-time")]
    public async Task<IActionResult> GetAverageResolutionTime()
    {
        var query = new ReportsAverageResolutionTimeQuery();
        var result = await mediator.Send(query);

        return Ok(new
        {
            AverageDays = result
        });
    }
}