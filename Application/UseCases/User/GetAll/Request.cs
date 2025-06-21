using Domain.Records;
using MediatR;

namespace Application.UseCases.User.GetAll;

public record Request(
    int Page = 1,
    int PageSize = 10
) : IRequest<BaseResponse<Response>>;