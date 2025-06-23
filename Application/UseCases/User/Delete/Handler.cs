using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Delete;

public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit
) : IRequestHandler<Request, BaseResponse<object>>
{
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetWithParametersAsync(u => u.Id.Equals(request.Id), cancellationToken);
        if (user is null)
            return new BaseResponse<object>(403, "User not found");

        await userRepository.DeleteAsync(user,cancellationToken);
        await dbCommit.Commit(cancellationToken);
        return new BaseResponse<object>(200, "User deleted successfully");
    }
}
