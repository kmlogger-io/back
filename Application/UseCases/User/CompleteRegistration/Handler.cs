
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.CompleteRegistration;

public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;

    public Handler(IUserRepository userRepository, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithParametersAsync(u => u.Id.Equals(request.Id) &&
            u.TokenActivate.Equals(request.Token), cancellationToken);

        if (user is null)
            return new BaseResponse<object>(400, "User not found or invalid token");
        
        user.Activate();
        user.UpdatePassword(new Password(request.Password));
        if (!user.IsValid)
            return new BaseResponse<object>(400, "Invalid data provided", user.Notifications.ToList());

        _userRepository.Update(user);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<object>(200, "User registration completed successfully");
    }
}
