using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Activate;

/// <summary>
/// Handles user account activation.
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
    /// Activates a user account if the token and email match a registered user.
    /// </summary>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.token == Guid.Empty)
            return new BaseResponse<Response>(400, "Invalid token");

        var user = await _userRepository.ActivateUserAsync(request.email, request.token, cancellationToken);
        if (user is null)
            return new BaseResponse<Response>(400, "Invalid email or token");

        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<Response>(200, "User successfully activated", new Response());
    }
}
