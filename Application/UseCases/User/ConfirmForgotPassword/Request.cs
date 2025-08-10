using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ConfirmForgotPassword;

public record  Request(Guid token, Guid id, string password) 
    : IRequest<BaseResponse<object>>;
