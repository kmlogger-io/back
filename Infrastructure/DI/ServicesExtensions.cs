using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

using ILogRepositoryHot = Domain.Interfaces.Repositories.Hot.ILogRepository;
using ILogRepositoryCold = Domain.Interfaces.Repositories.Cold.ILogRepository;
using LogRepositoryCold = Infrastructure.Repositories.Cold.LogRepository;
using LogRepositoryHot = Infrastructure.Repositories.Hot.LogRepository;

using Infrastructure.Repositories.Cold;
using Domain.Interfaces.Repositories.Cold;
using Infrastructure.Data.Cold;
using Infrastructure.Data.Hot;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure.DI;

public static class ServicesExtensions
{
    public static void ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IDbCommit, DbCommit>();
        services.AddScoped<Domain.Interfaces.Repositories.Hot.IDbCommit, Repositories.Hot.DbCommit>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ILogRepositoryHot, LogRepositoryHot>();
        services.AddScoped<ILogRepositoryCold, LogRepositoryCold>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAppRepository, AppRepository>();
        services.AddHttpClient();
    }

    public static void AddDataContexts(this IServiceCollection services)
    {
        services
            .AddDbContext<HotDbContext>(
                x => { x.UseSqlite(Configuration.SqliteConnectionString); });

        services
            .AddDbContext<ColdDbContext>(
                x => { x.UseNpgsql(StringConnection.BuildConnectionString()); });
    }
}
