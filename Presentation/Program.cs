using Domain;
using Infrastructure.DI;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Presentation.Common.Api;
using Presentation.Middlewares;
using Presentation.Common.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5070, o => o.Protocols = HttpProtocols.Http1); 
});

builder.AddConfiguration();
builder.AddSecurity();
builder.AddCrossOrigin();
builder.Services.AddDataContexts();
builder.AddServices();

var app = builder.Build();

// var key = Environment.GetEnvironmentVariable("KEY_KMLOGGER");
// var endpointUrl = "https://url-do-endpoint/validate"; 
// var isValid = await KeyValidatorService.ValidateKeyAsync(endpointUrl, key);
// if (!isValid)
// {
//     Console.WriteLine("Key inválida ou não definida. Encerrando aplicação.");
//     return;
// }

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseRouting();
app.UseMiddleware<ExceptionHandler>();
app.UseCors(Configuration.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();