using System;

namespace Domain.Interfaces.BackGroundServices;

public interface ILogTransferService
{
    Task TransferLogsAsync(CancellationToken stoppingToken);
}