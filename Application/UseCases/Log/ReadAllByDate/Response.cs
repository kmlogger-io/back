namespace Application.UseCases.Log.ReadAllByDate;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
