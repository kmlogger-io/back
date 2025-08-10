namespace Application.UseCases.App.Read.ReadAll
{
    public record Response(Guid Id, string? Name, string? CategoryName, string? Environment);
}