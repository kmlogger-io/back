using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.ForgotPassword.Activate;

/// <summary>
/// Handles password reset activation using a token.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;

    public Handler(IUserRepository userRepository, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _dbCommit = dbCommit;
    }

    /// <summary>
    /// Resets the user's password if the token is valid.
    /// </summary>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetWithParametersAsync(
            u => u.TokenActivate.Equals(request.token),
            cancellationToken
        );

        if (userFromDb is null)
            return new BaseResponse<Response>(400, "Invalid or expired token");

        userFromDb.UpdatePassword(new Password(request.newPassword));
        if (!userFromDb.IsValid)
            return new BaseResponse<Response>(400, "Invalid password", null, userFromDb.Notifications.ToList());

        userFromDb.ClearToken();
        _userRepository.Update(userFromDb);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<Response>(200, "Password reset successful", new Response());
    }
}
