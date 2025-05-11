namespace Application.UseCases.Log.Hot.Read.ReadById;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
