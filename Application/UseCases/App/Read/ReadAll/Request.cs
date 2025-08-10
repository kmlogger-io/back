using Domain.Records;
using MediatR;

namespace Application.UseCases.App.Read.ReadAll
{
    public record Request(
        int skip = 0,
        int take = 1000
    ) : IRequest<BaseResponse<List<Response>>>;
}
