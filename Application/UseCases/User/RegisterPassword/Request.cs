using Domain.Records;
using MediatR;

namespace Application.UseCases.User.RegisterPassword;

public record Request(
    string Email,
    string Password
) : IRequest<BaseResponse<Response>>;
