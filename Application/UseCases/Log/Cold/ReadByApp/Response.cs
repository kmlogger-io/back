namespace Application.UseCases.Log.Read.Cold.ReadByApp;

public record Response(Guid Id, Guid AppId, string Message, string Level, DateTime CreatedDate);
