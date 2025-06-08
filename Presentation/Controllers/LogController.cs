using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Records;
using Presentation.Common;
using Swashbuckle.AspNetCore.Annotations;

using CreateLogRequest = Application.UseCases.Log.Create.Request;
using CreateLogResponse = Application.UseCases.Log.Create.Response;
using ReadByAppLogRequest = Application.UseCases.Log.Read.ReadByApp.Request;
using ReadByAppLogResponse = Application.UseCases.Log.Read.ReadByApp.Response;
using ReadByIdLogRequest = Application.UseCases.Log.Read.ReadById.Request;
using ReadAllByDateLogRequest = Application.UseCases.Log.ReadAllByDate.Request;
using ReadAllByDateLogResponse = Application.UseCases.Log.ReadAllByDate.Response;
using ReadByIdLogResponse = Application.UseCases.Log.Read.ReadById.Response;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsible for managing  log entries.
/// </summary>
[ApiController]
[Route("logs")]
[Authorize]
public class LogController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new log entry in the  database.
    /// </summary>
    [HttpPost("create")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "LogCreate")]
    [ProducesResponseType(typeof(BaseResponse<CreateLogResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<BaseResponse<CreateLogResponse>>> CreateLog(
        [FromBody] CreateLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves all logs created today from the  database.
    /// </summary>
    [HttpGet("read-all-by-date")]
    [SwaggerOperation(OperationId = "LogReadAllToday")]
    [ProducesResponseType(typeof(BaseResponse<List<ReadAllByDateLogResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ReadAllByDateLogResponse>>>> ReadAllToday(
        [FromQuery] ReadAllByDateLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves logs from the  database filtered by AppId and date range.
    /// </summary>
    [HttpGet("read-by-app")]
    [SwaggerOperation(OperationId = "LogReadByApp")]
    [ProducesResponseType(typeof(BaseResponse<List<ReadByAppLogResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ReadByAppLogResponse>>>> ReadByApp(
        [FromQuery] ReadByAppLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves a single log from the  database by its Id.
    /// </summary>
    [HttpGet("read-by-id")]
    [SwaggerOperation(OperationId = "LogReadById")]
    [ProducesResponseType(typeof(BaseResponse<ReadByIdLogResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<ReadByIdLogResponse>>> ReadById(
        [FromQuery] ReadByIdLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}