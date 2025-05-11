using System;
using Domain.Interfaces.BackGroundServices;

namespace Application.BackGround;

public class LogTransferJob
{
    private readonly ILogTransferService _logTransferService;
    public LogTransferJob(ILogTransferService logTransferService)
    {
        _logTransferService = logTransferService;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    => await _logTransferService.TransferLogsAsync(cancellationToken);
}