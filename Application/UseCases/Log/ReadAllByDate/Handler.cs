using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.ReadAllByDate;

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
            x => x.CreatedDate.Date.Equals(request.Date.Date),
            cancellationToken
        );

        if (logs is null || !logs.Any())
            return new BaseResponse<List<Response>>(404, "No logs found for the specified date");

        var dto = logs.Select(log => new Response(
            log.Id,
            log.AppId,
            log.Message.Text,
            log.Level.ToString(),
            log.CreatedDate
        )).ToList();
        
        return new BaseResponse<List<Response>>(200, "Logs found", dto);
    }
}
