using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Cold.Read.ReadByInterval;

public record Request(
    DateTime StartDate,
    DateTime EndDate,
    int skip = 0,
    int take = 1000
) : IRequest<BaseResponse<List<Response>>>;
