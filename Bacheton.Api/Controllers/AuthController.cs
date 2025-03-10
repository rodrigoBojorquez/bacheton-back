using System.Security.Claims;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Common.Auth;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Application.User.Commands.Register;
using Bacheton.Application.User.Queries.Login;
using Bacheton.Domain.Constants;
using Bacheton.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly IMediator _mediator;
    private readonly ITokenService _tokenService;
    private readonly IAuthUtilities _authUtilities;

    public AuthController(IMediator mediator, ITokenService tokenService, IAuthUtilities authUtilities)
    {
        _mediator = mediator;
        _tokenService = tokenService;
        _authUtilities = authUtilities;
    }

    public record LoginRequest(string Email, string Password);

    public record RegisterRequest(string Name, string Email, string Password);


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _mediator.Send(new LoginQuery(request.Email, request.Password));

        return result.Match(authResult =>
            {
                _authUtilities.SetRefreshToken(authResult.RefreshToken);
                return Ok(new AccessToken(authResult.AccessToken));
            },
            Problem);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.Name, request.Email, request.Password);
        var result = await _mediator.Send(command);

        return result.Match(authResult =>
        {
            _authUtilities.SetRefreshToken(authResult.RefreshToken);
            return Ok(new AccessToken(authResult.AccessToken));
        }, Problem);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Token()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            return Problem(Errors.User.MissingRefreshToken);

        if (!await _tokenService.ValidateRefreshTokenAsync(refreshToken))
            return Problem(Errors.User.InvalidRefreshToken);

        var result = await _tokenService.RefreshToken(refreshToken);

        return result.Match(authResult =>
        {
            _authUtilities.SetRefreshToken(authResult.RefreshToken);
            return Ok(new AccessToken(authResult.AccessToken));
        }, Problem);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            return Problem(Errors.User.MissingRefreshToken);

        await _tokenService.DeleteRefreshTokenAsync(refreshToken);
        return Ok();
    }

    [HttpPost("show-access")]
    public async Task<IActionResult> ShowAccess()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Problem(Errors.User.Unauthorized);

        var result = await _authUtilities.ShowAccessLevel(Guid.Parse(userId));

        return result.Match(Ok, Problem);
    }
}