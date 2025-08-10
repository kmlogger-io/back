using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.Records.DTOS;
using MediatR;
using System.Linq.Expressions;

namespace Application.UseCases.User.GetAll;

public class Handler(
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<Request, BaseResponse<PaginatedResult<UserDto>>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<BaseResponse<PaginatedResult<UserDto>>> Handle(Request request, CancellationToken cancellationToken)
    {
        Expression<Func<Domain.Entities.User, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower();
            filter = u => u.FullName.FirstName.ToLower().Contains(searchTerm) || 
                         u.Email.Address.ToLower().Contains(searchTerm);
        }

        Expression<Func<Domain.Entities.User, object>>? orderBy = null;
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            orderBy = request.SortBy.ToLower() switch
            {
                "name" => u => u.FullName.FirstName,
                "email" => u => u.Email,
                "createdat" => u => u.CreatedDate,
                "active" => u => u.Active,
                _ => u => u.CreatedDate
            };
        }

        var skip = (request.Page - 1) * request.PageSize;
        var totalCount = await _userRepository.CountAsync(filter, cancellationToken);
        
        if (totalCount == 0)
            return new BaseResponse<PaginatedResult<UserDto>>(404, "No users found");

        var users = await _userRepository.GetAllWithParametersAsync(
            filter: filter,
            cancellationToken: cancellationToken,
            skip: skip,
            take: request.PageSize,
            includes: u => u.Roles,
            orderBy: orderBy,
            ascending: request.SortOrder?.ToLower() == "asc"
        );

        if (users is null || !users.Any())
            return new BaseResponse<PaginatedResult<UserDto>>(404, "No users found");

        var mappedUsers = _mapper.Map<List<UserDto>>(users.ToList());
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paginatedResult = new PaginatedResult<UserDto>(
            Data: mappedUsers,
            TotalCount: totalCount,
            Page: request.Page,
            PageSize: request.PageSize,
            TotalPages: totalPages
        );
        return new BaseResponse<PaginatedResult<UserDto>>(200, "Users retrieved successfully", paginatedResult);
    }
}