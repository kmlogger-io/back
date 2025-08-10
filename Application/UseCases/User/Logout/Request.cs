using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Logout;

public record Request(string email) : IRequest<BaseResponse<Response>>;
