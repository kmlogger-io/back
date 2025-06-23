namespace Domain.Interfaces.Services;

public interface IKmCentralService
{
    Task SendEmailQueueAsync(
        string email,
        string subject,
        string body,
        string? from = null,
        string? replyTo = null,
        bool isHtml = true
    );
}
