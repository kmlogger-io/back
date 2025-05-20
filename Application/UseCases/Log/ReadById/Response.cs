namespace Application.UseCases.Log.Read.ReadById;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
