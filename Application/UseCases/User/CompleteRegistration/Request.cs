using Domain.Records;
using MediatR;

namespace Application.UseCases.User.CompleteRegistration;

public record Request(Guid Id, Guid Token, string Password) : IRequest<BaseResponse<object>>;
