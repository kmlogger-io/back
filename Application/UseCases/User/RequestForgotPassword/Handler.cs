using System;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.RequestForgotPassword;

public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit
) : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IDbCommit _dbCommit = dbCommit;
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetWithParametersAsync(
            u => u.Email.Address!.Equals(request.email), cancellationToken
        );

        if (user is null || string.IsNullOrEmpty(user.Email.Address) || !user.Active)
            return new BaseResponse<object>(404, "User not found or email not registered");

        user.GenerateNewToken();
        _userRepository.Update(user);
        await _dbCommit.Commit(cancellationToken);

        var confirmationLink = Configuration.FrontendUrl +
            $"/reset-password?id={user.Id}&token={user.TokenActivate}";

        /*await kmCentralService.SendEmailQueueAsync(
            user.Email.Address, 
            "Password Reset Request",
            $"Hello {user.FullName}, you requested a password reset. Please click the link below
            $"to reset your password: {confirmationLink}"
        );*/
        return new BaseResponse<object>(200, "Password reset link sent successfully");
    }
}
