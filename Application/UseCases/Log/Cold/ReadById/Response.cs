namespace Application.UseCases.Log.Cold.Read.ReadById;

public record Response(
    Guid Id,
    Guid AppId,
    string Message,
    string Level,
    DateTime CreatedDate
);
