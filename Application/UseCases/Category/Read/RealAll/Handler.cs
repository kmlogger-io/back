using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Read.RealAll;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public Handler(ICategoryRepository repository)
    {
        _categoryRepository = repository;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllWithParametersAsync(null, cancellationToken, request.Skip, request.Take);
        var result = categories
            .Select(c => new Response(c.Id, c.Name.Name))
            .ToList();
        return new BaseResponse<List<Response>>(200, "Categories retrieved successfully", result);
    }
}
