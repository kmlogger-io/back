using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Activate;

/// <summary>
/// Represents a request to activate a user account.
/// </summary>
public record Request(
    string email,
    Guid token
) : IRequest<BaseResponse<Response>>;
public record Response();