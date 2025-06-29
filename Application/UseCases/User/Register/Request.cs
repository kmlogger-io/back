using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.Register;

public record Request(
    string FirstName,
    string? LastName,
    string Email,
    List<Guid> RolesId
) : IRequest<BaseResponse<Response>>; 