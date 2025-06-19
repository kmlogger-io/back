using System;
using Domain;

namespace Presentation.Common.Api;

public static class AppExtensions
{
        public static void ConfigureDevEnvironment(this WebApplication app)
        {
            app.UseForwardedHeaders();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"KMLogger API {Configuration.VersionApi}");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "KMLogger API Documentation";
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
            });
        }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}