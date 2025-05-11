namespace Application.UseCases.App.Create
{
    public record Response(Guid AppId, string Name, string CategoryName, string Environment);
}
