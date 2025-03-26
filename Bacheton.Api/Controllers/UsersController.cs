using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Application.User.Commands.Add;
using Bacheton.Application.User.Commands.Edit;
using Bacheton.Application.User.Common;
using Bacheton.Application.User.Queries.List;
using Bacheton.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class UsersController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly IAuthUtilities _authUtilities;

    public UsersController(IMediator mediator, IUserRepository userRepository, IAuthUtilities authUtilities)
    {
        _mediator = mediator;
        _userRepository = userRepository;
        _authUtilities = authUtilities;
    }

    public record ListUsersRequest(int Page = 1, string? Search = null, int PageSize = 10);

    public record AddUserRequest(string Name, string Email, string Password, Guid RoleId);

    public record UpdateUserRequest(Guid Id, string Name, string Email, string Password, Guid RoleId);

    [HttpGet]
    [RequiredPermission("read:Usuarios")]
    public async Task<IActionResult> List([FromQuery] ListUsersRequest request)
    {
        var query = new ListUsersQuery(request.Page, request.PageSize, request.Search);
        var result = await _mediator.Send(query);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var userId = _authUtilities.GetUserId();

        if (userId != id && !_authUtilities.HasSuperAccess()) return NotFound();

        var user = await _userRepository.IncludeRoleAsync(id);

        if (user is null) return Problem(Errors.User.NotFound);

        return Ok(new UserResult(user.Id, user.Name, user.Email, user.Role.Name, user.RoleId));
    }

    [HttpPost]
    [RequiredPermission("create:Usuarios")]
    public async Task<IActionResult> Add(AddUserRequest request)
    {
        var command = new AddUserCommand(request.Name, request.Email, request.Password, request.RoleId);
        var result = await _mediator.Send(command);

        return result.Match(created => Ok(created), Problem);
    }

    [HttpDelete("{id}")]
    [RequiredPermission("delete:Usuarios")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is not null)
            await _userRepository.DeleteAsync(user.Id);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserRequest request)
    {
        var command = new EditUserCommand(request.Id, request.Name, request.Email, request.Password, request.RoleId);
        var result = await _mediator.Send(command);

        return result.Match(updated => Ok(updated), Problem);
    }
}