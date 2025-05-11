namespace Application.UseCases.Log.Cold.Read.ReadByInterval;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
