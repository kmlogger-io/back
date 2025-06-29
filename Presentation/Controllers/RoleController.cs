using Domain.Records;
using Domain.Records.DTOS;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using Swashbuckle.AspNetCore.Annotations;
using GetAllRolesRequest = Application.UseCases.Role.GetAll.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("Role")]
public class RoleController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResult<RoleDto>>), StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = "RoleGetAll")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BaseResponse<PaginatedResult<RoleDto>>>> GetAll(
        [FromQuery] GetAllRolesRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
