using Domain.Interfaces.Repositories.Cold;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Category.Create;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;

    public Handler(ICategoryRepository categoryRepository, IDbCommit dbCommit)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var exists = await _categoryRepository.GetAllWithParametersAsync(
            x => x.Name!.Name.Trim().Equals(request.name.Trim()),
            cancellationToken);

        if (exists is not null)
            return new BaseResponse<Response>(400, "Category already exists");

        var category = new Domain.Entities.Category(new UniqueName(request.name), true);

        if (category.Notifications.Any())
            return new BaseResponse<Response>(400, "Errors occurred while creating category", null, category.Notifications.ToList());

        await _categoryRepository.CreateAsync(category, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        var response = new Response(category.Id, category.Name.Name);
        return new BaseResponse<Response>(200, "Category created successfully", response);
    }
}
