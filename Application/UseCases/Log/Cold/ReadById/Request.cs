using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Cold.Read.ReadById;

public record Request(Guid Id) : IRequest<BaseResponse<Response>>;
