using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middlewares;

using LoginRequest = Application.UseCases.User.Login.Request;
using RegisterRequest = Application.UseCases.User.Register.Request;
using RegisterResponse = Application.UseCases.User.Register.Response;
using ActivateRequest = Application.UseCases.User.Activate.Request; 
using ActivateResponse = Application.UseCases.User.Activate.Response;
using ActivatePasswordRequest = Application.UseCases.User.ForgotPassword.Activate.Request;
using ActivatePasswordResponse = Application.UseCases.User.ForgotPassword.Activate.Response;
using ForgotPasswordRequest = Application.UseCases.User.ForgotPassword.Request;
using ForgotPasswordResponse = Application.UseCases.User.ForgotPassword.Response;

using LoginResponse = Application.UseCases.User.Login.Response;

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
    [ProducesResponseType(typeof(BaseResponse<LoginResponse>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BaseResponse<LoginResponse>>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Registers a new user and sends an activation email.
    /// </summary>
    [HttpPost("Register")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "UserRegister")]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<RegisterResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResponse<RegisterResponse>>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Activates a user account using the email and activation token.
    /// </summary>
    [HttpPut("Activate-Account")]
    [SwaggerOperation(OperationId = "UserActivate")]
    [ProducesResponseType(typeof(BaseResponse<ActivateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ActivateResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResponse<ActivateResponse>>> ActivateAccount(
        [FromBody] ActivateRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Sends a password reset token to the user's email address.
    /// </summary>
    [HttpPost("Forgot-Password")]
    [SwaggerOperation(OperationId = "UserForgotPassword")]
    [ProducesResponseType(typeof(BaseResponse<ForgotPasswordResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<ForgotPasswordResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BaseResponse<ForgotPasswordResponse>>> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Resets the user's password using the provided reset token.
    /// </summary>
    [HttpPut("Forgot-Password/Activate")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "UserForgotPasswordActivate")]
    [ProducesResponseType(typeof(BaseResponse<ActivatePasswordResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseResponse<ActivatePasswordResponse>>> ActivateForgotPassword(
        [FromBody] ActivatePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}