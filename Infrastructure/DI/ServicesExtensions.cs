using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Net.Http.Headers;

namespace Infrastructure.DI;

public static class ServicesExtensions
{
    public static void ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IDbCommit, DbCommit>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAppRepository, AppRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IKmCentralService, KmCentralService>();
        services.AddHttpClient("KmCentral", client =>
        {
            client.BaseAddress = new Uri(Configuration.KmloggerCentralUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("X-API-KEY", Configuration.KEY_KMLOGGER);
            client.Timeout = TimeSpan.FromSeconds(60);
        });
    }

    public static void AddDataContexts(this IServiceCollection services)
    {
        services
            .AddDbContext<KmloggerDbContext>(
                x => { x.UseNpgsql(StringConnection.BuildConnectionString()); });
    }
}
