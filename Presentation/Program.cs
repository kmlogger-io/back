using Domain;
using Infrastructure.DI;
using Presentation.Common.Api;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);


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
{
    app.ConfigureDevEnvironment();
}
else
{
    app.UseCors(Configuration.CorsPolicyName);
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseMiddleware<ExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();