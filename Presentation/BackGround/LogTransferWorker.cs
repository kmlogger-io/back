using Application.BackGround;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Presentation.BackGround;

public class LogTransferWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LogTransferWorker> _logger;

    public LogTransferWorker(IServiceScopeFactory scopeFactory, ILogger<LogTransferWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Iniciando job de transferência de logs...");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var job = scope.ServiceProvider.GetRequiredService<LogTransferJob>();
                await job.ExecuteAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante execução do job.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
