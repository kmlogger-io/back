using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Hot.Read.ReadAllToday;

public record Request(
    int skip = 0,
    int take = 1000
) : IRequest<BaseResponse<List<Response>>>;
