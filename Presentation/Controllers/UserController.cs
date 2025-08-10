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
using GetAllRequest = Application.UseCases.User.GetAll.Request;
using CompleteRegistrationRequest = Application.UseCases.User.CompleteRegistration.Request;
using DeleteRequest = Application.UseCases.User.Delete.Request;
using RequestForgotPasswordRequest = Application.UseCases.User.RequestForgotPassword.Request;
using ConfirmForgotPasswordRequest = Application.UseCases.User.ConfirmForgotPassword.Request;
using UpdateUserRequest = Application.UseCases.User.Update.Request;

using Swashbuckle.AspNetCore.Annotations;
using Domain.Records;
using Presentation.Common;
using Domain.Records.DTOS;
using System.Security.Claims;

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
        CancellationToken cancellationToken)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        var response = await mediator.Send(new LogoutRequest(email), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("Register")]
    [SwaggerOperation(OperationId = "UserRegister")]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status200OK)]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BaseResponse<RegisterResponse>>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("Update")]
    [SwaggerOperation(OperationId = "UserUpdate")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BaseResponse<object>>> Update(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("GetAll")]
    [SwaggerOperation(OperationId = "UserGetAll")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResult<UserDto>>), StatusCodes.Status200OK)]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BaseResponse<PaginatedResult<UserDto>>>> GetAll(
        [FromQuery] GetAllRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("Delete")]
    [SwaggerOperation(OperationId = "UserDelete")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<BaseResponse<object>>> Delete(
        [FromQuery] Guid id,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("CompleteRegistration")]
    [SwaggerOperation(OperationId = "UserCompleteRegistration")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponse<object>>> CompleteRegistration(
        [FromBody] CompleteRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("RequestForgotPassword")]
    [SwaggerOperation(OperationId = "UserRequestForgotPassword")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponse<object>>> RequestForgotPassword(
        [FromQuery] RequestForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("ConfirmForgotPassword")]
    [SwaggerOperation(OperationId = "UserConfirmForgotPassword")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<BaseResponse<object>>> ConfirmForgotPassword(
        [FromQuery] ConfirmForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}