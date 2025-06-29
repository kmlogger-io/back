using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.Update;

public class Handler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IDbCommit dbCommit
) : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IDbCommit _dbCommit = dbCommit; 
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithParametersAsyncWithTracking(u => u.Id.Equals(request.userId),
            cancellationToken);
        if (user is null)
            return new BaseResponse<object>(404, "User not found");

        user.SetRoles(await _roleRepository.GetAllByIdsAsync(request.RolesId, cancellationToken));
        user.UpdateName(new FullName(request.FirstName, request.LastName));

        if (!user.IsValid)
            return new BaseResponse<object>(400, "Invalid Roles or Name", null, user.Notifications.ToList());

        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<object>(200, "User updated succesfuylly");
    }
}
