using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;


using Infrastructure.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain;
using ClickHouse.EntityFrameworkCore.Extensions;

namespace Infrastructure.DI;

public static class ServicesExtensions
{
    public static void ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IDbCommit, DbCommit>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAppRepository, AppRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddHttpClient();
    }

    public static void AddDataContexts(this IServiceCollection services)
    {
        services
            .AddDbContext<KmloggerDbContext>(
                x => { x.UseClickHouse(Configuration.ClickHouseConnectionString); });
    }
}
