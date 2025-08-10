using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Login;
public record Request(string email, string password) : IRequest<BaseResponse<Response>>;