using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

using LoginRequest = Application.UseCases.User.Login.Request;
using LoginResponse = Application.UseCases.User.Login.Response;
using RefreshTokenRequest = Application.UseCases.User.RefreshToken.Request;
using RefreshTokenResponse = Application.UseCases.User.RefreshToken.Response;
using LogoutRequest = Application.UseCases.User.Logout.Request;
using LogoutResponse = Application.UseCases.User.Logout.Response;

using Swashbuckle.AspNetCore.Annotations;
using Domain.Records;
using Presentation.Common;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável pelas operações de autenticação e gerenciamento de usuários.
/// </summary>
[ApiController]
[Route("User")]
public class UserController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    [HttpPost("Login")]
    [SwaggerOperation(OperationId = "UserLogin")]
    [ProducesResponseType(typeof(BaseResponse<LoginResponse>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<LoginResponse>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BaseResponse<LoginResponse>>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("RefreshToken")]
    [SwaggerOperation(OperationId = "UserRefreshToken")]
    [ProducesResponseType(typeof(BaseResponse<RefreshTokenResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<RefreshTokenResponse>>> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }


    [HttpPost("LogoutByEmail")]
    [SwaggerOperation(OperationId = "UserLogout")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<LogoutResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<LogoutResponse>>> Logout(
        [FromBody] LogoutRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}