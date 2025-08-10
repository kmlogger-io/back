using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.ReadAllByDate;

public record Request(
    DateTime Date
) : IRequest<BaseResponse<List<Response>>>;