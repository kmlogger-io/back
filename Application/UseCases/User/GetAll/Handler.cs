using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.GetAll;

public class Handler(
    IUserRepository userRepository
)
 : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllWithParametersAsync(
            filter: null, 
            cancellationToken: cancellationToken,
            skip: (request.Page - 1) * request.PageSize,
            take: request.PageSize,
            includes: u => u.Roles 
        );

        if (users is null || !users.Any())
            return new BaseResponse<Response>(404, "No users found");

        return new BaseResponse<Response>(200, "Users retrieved successfully", new Response(users));
    }
}
