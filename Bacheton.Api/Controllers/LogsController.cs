using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Logs.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class LogsController : ApiController
{
    private readonly IMediator _mediator;

    public LogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("error")]
    [AllowAnonymous]
    public Task<IActionResult> LogError()
    {
        throw new Exception("\u26a0\ufe0f Diabloooo!!!, se rompio la API \u26a0\ufe0f");
    }

    [HttpGet]
    [RequiredPermission("superAdmin:Administracion")]
    public async Task<IActionResult> List()
    {
        var query = new ListLogsQuery();
        return Ok(await _mediator.Send(query));
    }
}