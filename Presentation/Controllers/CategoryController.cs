using Domain.Records;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using Swashbuckle.AspNetCore.Annotations;

using CreateRequest = Application.UseCases.Category.Create.Request;
using CreateResponse = Application.UseCases.Category.Create.Response;
using DeleteRequest = Application.UseCases.Category.Delete.Request;
using DeleteResponse = Application.UseCases.Category.Delete.Response;
using ReadRequest = Application.UseCases.Category.Read.RealAll.Request;
using Response = Application.UseCases.Category.Read.RealAll.Response;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsible for managing categories.
/// </summary>
[ApiController]
[Route("Category")]
[Authorize]
public class CategoryController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Creates a new category.
    /// </summary>
    [HttpPost("Create")]
    [SwaggerOperation(OperationId = "CategoryCreate")]
    [ProducesResponseType(typeof(BaseResponse<CreateResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<CreateResponse>>> CreateCategory(
        [FromBody] CreateRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Deletes a category by ID.
    /// </summary>
    [HttpDelete("Delete")]
    [SwaggerOperation(OperationId = "CategoryDelete")]
    [ProducesResponseType(typeof(BaseResponse<DeleteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<DeleteResponse>>> DeleteCategory(
        [FromQuery] Guid id,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retrieves all categories with pagination.
    /// </summary>
    [HttpGet("Read")]
    [SwaggerOperation(OperationId = "CategoryReadAll")]
    [ProducesResponseType(typeof(BaseResponse<List<Response>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<List<Response>>>> Read(
        [FromQuery] int skip,
        [FromQuery] int take,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ReadRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}