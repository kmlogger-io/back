using Domain.Interfaces.Repositories.Cold;
using Domain.Records;
using MediatR;

namespace Application.UseCases.App.Read.ReadAll
{
    public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
    {
        private readonly IAppRepository _appRepository;

        public Handler(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var apps = await _appRepository.GetAll(cancellationToken, request.skip, request.take);
            var result = apps.Select(app => new Response(
                app.Id,
                app.Name.Name,
                app.Category.Name.Name,
                app.Environment.ToString()
            )).ToList();
            return new BaseResponse<List<Response>>(200, "Apps retrieved successfully", result);
        }
    }
}
