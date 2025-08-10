using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.Register;

public class Handler(
    IDbCommit dbCommit,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IKmCentralService  kmCentralService
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

        if (roles is null || !roles.Any()) 
            return new BaseResponse<Response>(404, "Roles not found");

        if (roles.Count() != request.RolesId.Count)
            return new BaseResponse<Response>(400, "Some roles were not found");

        var rolesList = roles.ToList();

        _roleRepository.AttachRange(rolesList);
        var newUser = new Domain.Entities.User(
            new FullName(request.FirstName, request.LastName),
            new Email(request.Email),
            rolesList
        );

        if (newUser.Notifications.Any())
            return new BaseResponse<Response>(400, "Request invalid", null, [.. newUser.Notifications]);

        await _userRepository.CreateAsync(newUser, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        var confirmationLink = Configuration.FrontendUrl +
            $"/confirm-registration?id={newUser.Id}&token={newUser.TokenActivate}";

        /*await kmCentralService.SendEmailQueueAsync(
            request.Email,
            "Welcome to the Kmlogger System",
            $"Hello {request.FullName}, welcome to our system! Complete your registration by clicking the link below."
            + $"<a href=\"{confirmationLink}\">Complete Registration</a>",
            isHtml: true
        );*/

        return new BaseResponse<Response>(201, "User created successfully. An email has been sent for con");
    }
}