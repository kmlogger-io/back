using RequestReadByAppCold = Application.UseCases.Log.Read.Cold.ReadByApp.Request;
using ResponseReadByAppCold = Application.UseCases.Log.Read.Cold.ReadByApp.Response;
using RequestReadByIdCold = Application.UseCases.Log.Cold.Read.ReadById.Request;
using ResponseReadByIdCold = Application.UseCases.Log.Cold.Read.ReadById.Response;
using RequestReadByIntervalCold = Application.UseCases.Log.Cold.Read.ReadByInterval.Request;
using ResponseReadByIntervalCold = Application.UseCases.Log.Cold.Read.ReadByInterval.Response;
using Microsoft.AspNetCore.Mvc;
using Domain.Records;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

public partial class LogController
{
    /// <summary>
    /// Retrieves logs from the cold database filtered by AppId and date range.
    /// </summary>
    [HttpGet("cold/read-by-app")]
    [SwaggerOperation(OperationId = "LogReadByAppCold")]
    [ProducesResponseType(typeof(BaseResponse<List<ResponseReadByAppCold>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ResponseReadByAppCold>>>> ReadByAppCold(
        [FromQuery] RequestReadByAppCold request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves a single log from the cold database by its Id.
    /// </summary>
    [HttpGet("cold/read-by-id")]
    [SwaggerOperation(OperationId = "LogReadByIdCold")]
    [ProducesResponseType(typeof(BaseResponse<ResponseReadByIdCold>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<ResponseReadByIdCold>>> ReadByIdCold(
        [FromQuery] RequestReadByIdCold request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves logs from the cold database within a date range.
    /// </summary>
    [HttpGet("cold/read-by-interval")]
    [SwaggerOperation(OperationId = "LogReadByIntervalCold")]
    [ProducesResponseType(typeof(BaseResponse<List<ResponseReadByIntervalCold>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<ResponseReadByIntervalCold>>>> ReadByIntervalCold(
        [FromQuery] RequestReadByIntervalCold request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}