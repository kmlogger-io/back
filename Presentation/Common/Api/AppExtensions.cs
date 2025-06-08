using System;

namespace Presentation.Common.Api;

public static class AppExtensions
{
    #region ConfigureEnvironment
    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseForwardedHeaders();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "KMLogger API");
            c.RoutePrefix = "swagger";
        });
    }
    #endregion ConfigureEnvironment

    #region Security
    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    #endregion
}