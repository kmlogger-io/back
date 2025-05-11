using Domain.Interfaces.Repositories.Hot;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Hot.Read.ReadAllToday;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ILogRepository _logRepository;

    public Handler(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var logs = await _logRepository.GetAllWithParametersAsync(
            x => x.CreatedDate >= today && x.CreatedDate < tomorrow,
            cancellationToken,
            request.skip,
            request.take
        );

        if (logs is null || !logs.Any())
            return new BaseResponse<List<Response>>(404, "No logs found for today");

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
