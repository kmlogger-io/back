using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using CreateRequest = Application.UseCases.App.Create.Request;
using CreateAppResponse = Application.UseCases.App.Create.Response;
using ReadAllRequest = Application.UseCases.App.Read.ReadAll.Request;   
using ReadAppResponse = Application.UseCases.App.Read.ReadAll.Response;
using Presentation.Common;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsible for managing applications.
/// </summary>
[ApiController]
[Route("app")]
[Authorize]
public class AppController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new application.
    /// </summary>
    [HttpPost("create")]
    [SwaggerOperation(OperationId = "AppCreate")]
    [ProducesResponseType(typeof(BaseResponse<CreateAppResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<CreateAppResponse>>> CreateApp(
        [FromBody] CreateRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves all registered applications with pagination.
    /// </summary>
    [HttpGet("read")]
    [SwaggerOperation(OperationId = "AppReadAll")]
    [ProducesResponseType(typeof(BaseResponse<List<ReadAppResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ReadAppResponse>>>> Read(
        [FromQuery] int skip,
        [FromQuery] int take,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ReadAllRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
