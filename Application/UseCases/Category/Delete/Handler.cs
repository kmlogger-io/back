using Domain.Interfaces.Repositories.Cold;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Delete;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;

    public Handler(ICategoryRepository repository, IDbCommit dbCommit)
    {
        _categoryRepository = repository;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetWithParametersAsync(
            x => x.Id.Equals(request.id), cancellationToken);

        if (category is null)
            return new BaseResponse<Response>(404, "Category not found");

        await _categoryRepository.DeleteAsync(category, cancellationToken);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<Response>(200, "Category deleted successfully", new Response());
    }
}
