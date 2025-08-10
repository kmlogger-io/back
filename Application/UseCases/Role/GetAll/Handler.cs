using System;
using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.Records.DTOS;
using MediatR;

namespace Application.UseCases.Role.GetAll;

public class Handler(
    IRoleRepository roleRepository,
    IMapper mapper
) : IRequestHandler<Request, BaseResponse<PaginatedResult<RoleDto>>>
{
    public async Task<BaseResponse<PaginatedResult<RoleDto>>> Handle(Request request, CancellationToken cancellationToken)
    {
        Expression<Func<Domain.Entities.Role, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.ToLower();
            filter = u => u.Name.Name.ToLower().Contains(searchTerm); 
        }

        Expression<Func<Domain.Entities.Role, object>>? orderBy = null;
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            orderBy = request.SortBy.ToLower() switch
            {
                "name" => u => u.Name.Name,
                "slug" => u => u.Slug,
                "createdat" => u => u.CreatedDate,
                _ => u => u.CreatedDate
            };
        }

        var skip = (request.Page - 1) * request.PageSize;
        var totalCount = await roleRepository.CountAsync(filter, cancellationToken);
        
        if (totalCount == 0)
            return new BaseResponse<PaginatedResult<RoleDto>>(404, "No roles found");

        var roles = await roleRepository.GetAllWithParametersAsync(
            filter: filter,
            cancellationToken: cancellationToken,
            skip: skip,
            take: request.PageSize,
            orderBy: orderBy,
            ascending: request.SortOrder?.ToLower() == "asc"
        );

        if (roles is null || !roles.Any())
            return new BaseResponse<PaginatedResult<RoleDto>>(404, "No roles found");

        var mappedRoles = mapper.Map<List<RoleDto>>(roles.ToList());
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paginatedResult = new PaginatedResult<RoleDto>(
            Data: mappedRoles,
            TotalCount: totalCount,
            Page: request.Page,
            PageSize: request.PageSize,
            TotalPages: totalPages
        );
        return new BaseResponse<PaginatedResult<RoleDto>>(200, "Roles retrieved successfully", paginatedResult);
    }
}
