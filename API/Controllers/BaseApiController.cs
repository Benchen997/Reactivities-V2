using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController: ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                      ?? throw new InvalidOperationException("Imediator not found");

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        return result switch
        {
            { IsSuccess: true, Value: not null } => Ok(result.Value),
            { IsSuccess: true, Value: null } => NotFound(),
            _ => BadRequest(result.Error)
        };
    }
}