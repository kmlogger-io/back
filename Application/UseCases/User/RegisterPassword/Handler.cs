using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.RegisterPassword;

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
        var userExists = await _userRepository.GetWithParametersAsync(
            u => u.Email.Address!.Equals(request.Email), cancellationToken
        );
        if (userExists is null)
            return new BaseResponse<Response>(404, "User not found");

        if (userExists.Password is not null)
            return new BaseResponse<Response>(400, "User already has a password");

        userExists.UpdatePassword(new Password(request.Password));
        if (userExists.Notifications.Any())
            return new BaseResponse<Response>(400, "Request invalid", null, [.. userExists.Notifications]);

        _userRepository.Update(userExists);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<Response>(200, "Password registered successfully", new Response(userExists.Id));
    }
}
