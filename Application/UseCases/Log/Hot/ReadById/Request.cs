using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Hot.Read.ReadById;
public record Request(Guid Id) : IRequest<BaseResponse<Response>>;
