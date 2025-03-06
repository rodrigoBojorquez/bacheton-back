using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Roles.Commands.Create;
using Bacheton.Application.Roles.Commands.Update;
using Bacheton.Application.Roles.Queries.List;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class RolesController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IRoleRepository _roleRepository;

    public RolesController(IMediator mediator, IRoleRepository roleRepository)
    {
        _mediator = mediator;
        _roleRepository = roleRepository;
    }

    public record ListRolesRequest(int Page, int PageSize, string? Search);
    public record CreateRoleRequest(string Name, List<Guid> Permissions, string? Description);
    public record UpdateRoleRequest(Guid Id, string Name, List<Guid> Permissions, string? Description);


    [HttpGet]
    [RequiredPermission("read:Roles")]
    public async Task<IActionResult> List([FromQuery] ListRolesRequest request)
    {
        var query = new ListRolesQuery(request.Page, request.PageSize, request.Search);
        var result = await _mediator.Send(query);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
    
    [HttpPost]
    [RequiredPermission("create:Roles")]
    public async Task<IActionResult> Create(CreateRoleRequest request)
    {
        var command = new CreateRoleCommand(request.Name, request.Permissions,request.Description);
        var result = await _mediator.Send(command);
        
        return result.Match(created => Ok(created), Problem);
    }
    
    [HttpPut]
    [RequiredPermission("update:Roles")]
    public async Task<IActionResult> Update(UpdateRoleRequest request)
    {
        var command = new UpdateRoleCommand(request.Id, request.Name, request.Permissions, request.Description);
        var result = await _mediator.Send(command);
        
        return result.Match(updated => Ok(updated), Problem);
    }
    
    [HttpDelete]
    [RequiredPermission("delete:Roles")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _roleRepository.DeleteAsync(id);
        return Ok(Result.Deleted);
    }
}