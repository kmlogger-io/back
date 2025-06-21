using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ResetPassword;

public class Handler(
    IDbCommit dbCommit,
    IUserRepository userRepository
) 
: IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IDbCommit _dbCommit = dbCommit;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithParametersAsync(
            u => u.Email.Address!.Equals(request.Email), cancellationToken
        );

        if (user is null)
            return new BaseResponse<Response>(404, "User not found");

        if (user.Password is null)
            return new BaseResponse<Response>(400, "User does not have a password");

        user.ResetPassword();
        _userRepository.Update(user);
        await _dbCommit.Commit(cancellationToken);  
        return new BaseResponse<Response>(200, "Password reset successfully", new Response(user.Id));

    }
}
