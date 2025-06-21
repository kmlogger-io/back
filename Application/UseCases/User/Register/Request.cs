using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.Register;

public record Request(
    FullName FullName,
    string Email,
    List<Guid> RolesId
) : IRequest<BaseResponse<Response>>; 