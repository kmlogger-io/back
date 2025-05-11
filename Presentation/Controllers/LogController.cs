using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Records;
using Presentation.Common;
using Swashbuckle.AspNetCore.Annotations;

using CreateLogRequest = Application.UseCases.Log.Hot.Create.Request;
using CreateLogResponse = Application.UseCases.Log.Hot.Create.Response;
using ReadAllTodayLogRequest = Application.UseCases.Log.Hot.Read.ReadAllToday.Request;
using ReadAllTodayLogResponse = Application.UseCases.Log.Hot.Read.ReadAllToday.Response;
using ReadByAppLogRequest = Application.UseCases.Log.Hot.Read.ReadByApp.Request;
using ReadByAppLogResponse = Application.UseCases.Log.Hot.Read.ReadByApp.Response;
using ReadByIdLogRequest = Application.UseCases.Log.Hot.Read.ReadById.Request;
using ReadByIdLogResponse = Application.UseCases.Log.Hot.Read.ReadById.Response;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsible for managing hot log entries.
/// </summary>
[ApiController]
[Route("logs")]
[Authorize]
public partial class LogController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new log entry in the hot database.
    /// </summary>
    [HttpPost("create")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "LogCreateHot")]
    [ProducesResponseType(typeof(BaseResponse<CreateLogResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<BaseResponse<CreateLogResponse>>> CreateLog(
        [FromBody] CreateLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves all logs created today from the hot database.
    /// </summary>
    [HttpGet("read-all-today")]
    [SwaggerOperation(OperationId = "LogReadAllTodayHot")]
    [ProducesResponseType(typeof(BaseResponse<List<ReadAllTodayLogResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ReadAllTodayLogResponse>>>> ReadAllToday(
        [FromQuery] ReadAllTodayLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves logs from the hot database filtered by AppId and date range.
    /// </summary>
    [HttpGet("read-by-app")]
    [SwaggerOperation(OperationId = "LogReadByAppHot")]
    [ProducesResponseType(typeof(BaseResponse<List<ReadByAppLogResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ReadByAppLogResponse>>>> ReadByApp(
        [FromQuery] ReadByAppLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves a single log from the hot database by its Id.
    /// </summary>
    [HttpGet("read-by-id")]
    [SwaggerOperation(OperationId = "LogReadByIdHot")]
    [ProducesResponseType(typeof(BaseResponse<ReadByIdLogResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<ReadByIdLogResponse>>> ReadById(
        [FromQuery] ReadByIdLogRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}