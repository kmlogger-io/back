namespace Application.UseCases.Log.Hot.Read.ReadAllToday;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
