using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ForgotPassword.Activate;

/// <summary>
/// Represents the request to reset a user's password using a token.
/// </summary>
public record Request(Guid? token, string newPassword) : IRequest<BaseResponse<Response>>;

public record Response();