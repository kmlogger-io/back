using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.ConfirmForgotPassword;

public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit
) : IRequestHandler<Request, BaseResponse<object>>
{
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetWithParametersAsync(
            u => u.Id!.Equals(request.id) && u.TokenActivate == request.token,
            cancellationToken
        );

        if (user is null || string.IsNullOrEmpty(user.Email.Address) || !user.Active)
            return new BaseResponse<object>(404, "User not found or email not registered");

        user.ConfirmForgotPassword(new Password(request.password));
        if (!user.IsValid)
            return new BaseResponse<object>(400, "Invalid data provided", user.Notifications.ToList());

        userRepository.Update(user);
        await dbCommit.Commit(cancellationToken);   
        return new BaseResponse<object>(200, "Password reset confirmed successfully");
    }
}
