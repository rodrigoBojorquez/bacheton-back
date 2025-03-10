using System.Linq.Expressions;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Permissions.Commands.Update;
using Bacheton.Application.Permissions.Queries.List;
using Bacheton.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class PermissionsController : ApiController
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public record ListPermissionsRequest(string? Search, Guid? RoleId);

    public record UpdatePermissionRequest(Guid Id, string DisplayName, string? Icon);

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListPermissionsRequest request)
    {
        var query = new ListPermissionsQuery(request.Search, request.RoleId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePermissionRequest request)
    {
        var command = new UpdatePermissionCommand(request.Id, request.DisplayName, request.Icon);
        var result = await _mediator.Send(command);

        return result.Match(s => Ok(s), Problem);
    }
}