using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Read.ReadByApp;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ILogRepository _logRepository;

    public Handler(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var logs = await _logRepository.GetAllWithParametersAsync(
            x => x.AppId == request.AppId &&
                 x.CreatedDate >= request.StartDate &&
                 x.CreatedDate <= request.EndDate,
            cancellationToken,
            request.skip,
            request.take
        );

        if (logs is null || !logs.Any())
            return new BaseResponse<List<Response>>(404, "Logs not found");

        var result = logs.Select(log => new Response(
            log.Id,
            log.AppId,
            log.Message.Text,
            log.Level.ToString(),
            log.CreatedDate
        )).ToList();

        return new BaseResponse<List<Response>>(200, "Logs found", result);
    }
}
