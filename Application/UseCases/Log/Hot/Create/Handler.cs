using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Hot;
using Domain.Records;
using MediatR;

using IAppRepository = Domain.Interfaces.Repositories.Cold.IAppRepository;

namespace Application.UseCases.Log.Hot.Create;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ILogRepository _logRepository;
    private readonly IMapper _mapper;
    private readonly IDbCommit _dbCommit;
    private readonly IAppRepository _appRepository;

    public Handler(ILogRepository logRepository, IMapper mapper, IDbCommit dbCommit, IAppRepository appRepository)
    {
        _logRepository = logRepository;
        _mapper = mapper;
        _dbCommit = dbCommit;
        _appRepository = appRepository;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        if (await _appRepository.GetWithParametersAsync(x => x.Id == request.AppId, cancellationToken) is null)
            return new BaseResponse<Response>(404, "App not found");

        var log = _mapper.Map<LogApp>(request);

        if (log.Notifications.Any())
            return new BaseResponse<Response>(400, "There were some problems when creating the log", null, log.Notifications.ToList());

        await _logRepository.CreateAsync(log, cancellationToken);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<Response>(201, "Log created successfully", new Response(log.Id));
    }
}
