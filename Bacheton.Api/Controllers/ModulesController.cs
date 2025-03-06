using Bacheton.Api.Common.Attributes;
using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class ModulesController : ApiController
{
    private readonly IModuleRepository _moduleRepository;

    public ModulesController(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    [HttpGet]
    [RequiredPermission("read:Roles")]
    public async Task<ActionResult> List()
    {
        var data = await _moduleRepository.ListAllAsync();
        return Ok(data);
    }
}