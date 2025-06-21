using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.Register;

public class Handler(
    IDbCommit dbCommit,
    IUserRepository userRepository,
    IRoleRepository roleRepository
) : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IDbCommit _dbCommit = dbCommit;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetWithParametersAsync(
            u => u.Email.Address!.Equals(request.Email), cancellationToken
            ) is not null)
            return new BaseResponse<Response>(400, "User already exists");

        var roles = await _roleRepository.GetAllWithParametersAsync(
            r => request.RolesId.Contains(r.Id),
            cancellationToken
        );
        if (roles is null) return new BaseResponse<Response>(404, "Roles not found");

        var newUser = new Domain.Entities.User(
            request.FullName,
            new Email(request.Email),
            roles
        );

        if (newUser.Notifications.Any())
            return new BaseResponse<Response>(400, "Request invalid", null, [.. newUser.Notifications]);

        await _userRepository.CreateAsync(newUser, cancellationToken);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<Response>(201, "User created succesfully");
    }
}
