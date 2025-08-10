using Domain.Enums;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Create;

public record Request(
    Guid AppId,
    string Message,
    string Level,
    string? StackTrace,
    string? Source,
    Domain.Enums.Environment Environment
) : IRequest<BaseResponse<Response>>;
