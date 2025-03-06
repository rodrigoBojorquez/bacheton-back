using Bacheton.Api.Common.Controllers;
using Bacheton.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bacheton.Api.Controllers;

public class PermissionsController : ApiController
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionsController(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var data = await _permissionRepository.ListAllAsync();
        
        return Ok(data);
    }
}