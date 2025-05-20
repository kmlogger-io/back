using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Read.ReadByApp;

public record Request(
    Guid AppId,
    DateTime StartDate,
    DateTime EndDate,
    int skip = 0,
    int take = 1000
) : IRequest<BaseResponse<List<Response>>>;
