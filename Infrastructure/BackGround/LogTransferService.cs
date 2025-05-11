using Domain.Entities;
using Domain.Interfaces.BackGroundServices;
using Infrastructure.Data.Cold;
using Infrastructure.Data.Hot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class LogTransferService(IServiceScopeFactory scopeFactory) : ILogTransferService
{
    public async Task TransferLogsAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var hotDbContext = scope.ServiceProvider.GetRequiredService<HotDbContext>();
        var coldDbContext = scope.ServiceProvider.GetRequiredService<ColdDbContext>();

        const int batchSize = 1000;
        DateTime? lastDate = null;

        while (true)
        {
            var query = hotDbContext.Logs
                .AsNoTracking()
                .OrderBy(x => x.CreatedDate)
                .Take(batchSize);

            if (lastDate is not null)
            {
                query = query.Where(x => x.CreatedDate > lastDate);
            }

            var logs = await query.ToListAsync(cancellationToken);
            if (logs.Count == 0)
                break;

            await coldDbContext.Logs.AddRangeAsync(logs, cancellationToken);
            await coldDbContext.SaveChangesAsync(cancellationToken);

            hotDbContext.Logs.RemoveRange(logs);
            await hotDbContext.SaveChangesAsync(cancellationToken);
            lastDate = logs.Last().CreatedDate;
        }
    }
}