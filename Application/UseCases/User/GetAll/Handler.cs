using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.Records.DTOS;
using MediatR;

namespace Application.UseCases.User.GetAll;

public class Handler(
    IUserRepository userRepository,
    IMapper mapper
)
 : IRequestHandler<Request, BaseResponse<List<UserDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<BaseResponse<List<UserDto>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllWithParametersAsync(
            filter: null,
            cancellationToken: cancellationToken,
            skip: (request.Page - 1) * request.PageSize,
            take: request.PageSize,
            includes: u => u.Roles
        );

        if (users is null || !users.Any())
            return new BaseResponse<List<UserDto>>(404, "No users found");
            
        return new BaseResponse<List<UserDto>>(200, "Users retrieved successfully",
            _mapper.Map<List<UserDto>>(users.ToList())); 
    }
}
