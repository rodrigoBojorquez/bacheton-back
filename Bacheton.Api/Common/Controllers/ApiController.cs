using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bacheton.Api.Common.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    private static readonly IReadOnlyDictionary<ErrorType, int> ErrorToStatusCodes = new Dictionary<ErrorType, int>()
    {
        { ErrorType.Conflict, StatusCodes.Status409Conflict },
        { ErrorType.Validation, StatusCodes.Status400BadRequest },
        { ErrorType.Unauthorized, StatusCodes.Status401Unauthorized },
        { ErrorType.Forbidden, StatusCodes.Status403Forbidden },
        { ErrorType.NotFound, StatusCodes.Status404NotFound },
    };

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items["Errors"] = JsonSerializer.Serialize(errors);

        return Problem(errors[0]);
    }

    protected IActionResult Problem(Error error)
    {
        var statusCode = ErrorToStatusCodes.GetValueOrDefault(error.Type, StatusCodes.Status500InternalServerError);

        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Status = statusCode,
            Title = error.Description,
            Extensions = { ["errorCode"] = error.Code }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelState);
    }
}