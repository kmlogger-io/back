using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Logout;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;

    public Handler(IUserRepository userRepository, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<Response>> Handle(Request request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.email))
            return new BaseResponse<Response>(401, "User not authenticated", null);

        var user = await _userRepository.GetByEmail(request.email, cancellationToken);
        
        if (user is null)
            return new BaseResponse<Response>(404, "User not found", null);

        user.ClearRefreshToken();
        await _userRepository.UpdateRefreshToken(user, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        return new BaseResponse<Response>(200, "User logged out successfully",
             new Response("Logout successful", DateTime.UtcNow)
        );
    }
}