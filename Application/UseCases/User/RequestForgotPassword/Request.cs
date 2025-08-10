using Domain.Records;
using MediatR;

namespace Application.UseCases.User.RequestForgotPassword;

public record Request(string email) 
    : IRequest<BaseResponse<object>>;
    