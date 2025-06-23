using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Delete;

public record Request(Guid Id) : IRequest<BaseResponse<object>>;