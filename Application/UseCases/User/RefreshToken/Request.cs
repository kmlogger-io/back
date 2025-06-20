using Domain.Records;
using MediatR;

namespace Application.UseCases.User.RefreshToken;

public record Request(
    string AccessToken,
    string RefreshToken
) : IRequest<BaseResponse<Response>>;
