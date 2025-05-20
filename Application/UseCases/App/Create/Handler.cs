using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.App.Create
{
    public class Handler : IRequestHandler<Request, BaseResponse<Response>>
    {
        private readonly IAppRepository _appRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDbCommit _dbCommit;

        public Handler(IAppRepository appRepository, ICategoryRepository categoryRepository, IDbCommit dbCommit)
        {
            _appRepository = appRepository;
            _categoryRepository = categoryRepository;
            _dbCommit = dbCommit;
        }

        public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (await _appRepository.GetWithParametersAsync(x => x.Name.Name.Equals(request.name), cancellationToken) is not null)
                return new BaseResponse<Response>(400, "App already exists");

            var category = await _categoryRepository.GetWithParametersAsync(x => x.Id.Equals(request.categoryId), cancellationToken);
            if (category is null)
                return new BaseResponse<Response>(404, "Category not found");

            var app = new Domain.Entities.App(
                new UniqueName(request.name),
                category,
                request.environment,
                null, true);

            if (app.Notifications.Any())
                return new BaseResponse<Response>(400, "Some problems occurred when creating app", null, 
                app.Notifications.ToList());

            await _appRepository.CreateAsync(app, cancellationToken);
            await _dbCommit.Commit(cancellationToken);

            var dto = new Response(app.Id, app.Name.Name, category.Name!.Name, app?.Environment?.ToString());
            return new BaseResponse<Response>(200, "App created successfully", dto);
        }
    }
}
