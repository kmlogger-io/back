using Domain.Records;
using Domain.Records.DTOS;
using MediatR;

namespace Application.UseCases.Role.GetAll;

public record Request(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? SortBy = null,
    string? SortOrder = "asc"
) : IRequest<BaseResponse<PaginatedResult<RoleDto>>>;
