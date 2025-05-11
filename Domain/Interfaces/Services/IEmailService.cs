using System;
using Domain.Records;

namespace Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken );
}
