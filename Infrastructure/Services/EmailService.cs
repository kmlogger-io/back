using System;
using System.Net;
using System.Net.Mail;
using Domain;
using Domain.Interfaces.Services;
using Domain.Records;

namespace Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        var smtp = new SmtpClient(Configuration.SmtpServer, Configuration.SmtpPort)
        {
            Credentials = new NetworkCredential(Configuration.SmtpUser, Configuration.SmtpPass),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(message.FromEmail, message.FromName),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = true
        };
        mail.To.Add(new MailAddress(message.To, message.ToName));
        await smtp.SendMailAsync(mail, cancellationToken);
    }
}