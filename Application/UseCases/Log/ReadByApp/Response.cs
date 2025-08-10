namespace Application.UseCases.Log.Read.ReadByApp;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
