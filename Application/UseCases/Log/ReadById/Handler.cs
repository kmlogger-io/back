using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Log.Read.ReadById;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ILogRepository _logRepository;

    public Handler(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var log = await _logRepository.GetWithParametersAsync(x => x.Id == request.Id, cancellationToken);
        if (log is null)
            return new BaseResponse<Response>(404, "Log not found");

        var dto = new Response(
            log.Id,
            log.AppId,
            log.Message.Text,
            log.Level.ToString(),
            log.CreatedDate
        );
        return new BaseResponse<Response>(200, "Log found", dto);
    }
}
