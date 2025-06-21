using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using LoginRequest = Application.UseCases.User.Login.Request;
using LoginResponse = Application.UseCases.User.Login.Response;
using RefreshTokenRequest = Application.UseCases.User.RefreshToken.Request;
using RefreshTokenResponse = Application.UseCases.User.RefreshToken.Response;
using LogoutRequest = Application.UseCases.User.Logout.Request;
using LogoutResponse = Application.UseCases.User.Logout.Response;
using RegisterResponse = Application.UseCases.User.Register.Response;
using RegisterRequest = Application.UseCases.User.Register.Request;
using ResetPasswordResponse = Application.UseCases.User.ResetPassword.Response;
using ResetPasswordRequest = Application.UseCases.User.ResetPassword.Request;
using RegisterPasswordRequest = Application.UseCases.User.RegisterPassword.Request;
using RegisterPasswordResponse = Application.UseCases.User.RegisterPassword.Response;
using GetAllResponse = Application.UseCases.User.GetAll.Response;
using GetAllRequest = Application.UseCases.User.GetAll.Request;


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


    [HttpPost("Logout")]
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

    [HttpPost("Register")]
    [SwaggerOperation(OperationId = "UserRegister")]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BaseResponse<RegisterResponse>>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("ResetPassword")]
    [SwaggerOperation(OperationId = "UserResetPassword")]
    [ProducesResponseType(typeof(BaseResponse<ResetPasswordResponse>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BaseResponse<ResetPasswordResponse>>> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("RegisterPassword")]
    [SwaggerOperation(OperationId = "UserRegisterPassword")]
    [ProducesResponseType(typeof(BaseResponse<RegisterPasswordResponse>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponse<RegisterPasswordResponse>>> RegisterPassword(
        [FromBody] RegisterPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("GetAll")]
    [SwaggerOperation(OperationId = "UserGetAll")]
    [ProducesResponseType(typeof(BaseResponse<GetAllResponse>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BaseResponse<GetAllResponse>>> GetAll(
        [FromQuery] GetAllRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}