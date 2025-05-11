
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Register;
public record Request(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    long? Number,
    string? NeighBordHood,
    string? Road,
    string? Complement,
    string Password
) : IRequest<BaseResponse<Response>>;

public record Response();
