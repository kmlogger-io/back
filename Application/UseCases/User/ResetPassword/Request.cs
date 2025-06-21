using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ResetPassword;

public record Request(
    string Email
) : IRequest<BaseResponse<Response>>;
