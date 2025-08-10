using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.Update;

public record Request(
    Guid userId,
    List<Guid> RolesId,
    string? FirstName,
    string? LastName
) : IRequest<BaseResponse<object>>;
