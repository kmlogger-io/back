using System;
using System.Net.Http.Json;
using Domain;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public sealed class KmCentralService(IHttpClientFactory httpClientFactory) : IKmCentralService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    public async Task SendEmailQueueAsync(string email, string subject, string body, string? from = null,
         string? replyTo = null, bool isHtml = true)
    {
        if (string.IsNullOrWhiteSpace(Configuration.KmloggerCentralUrl))
            throw new InvalidOperationException("KmCentral URL is not configured.");

        if (string.IsNullOrWhiteSpace(Configuration.KEY_KMLOGGER))
            throw new InvalidOperationException("KmCentral API Key is not configured.");

        var client = _httpClientFactory.CreateClient("KmCentral");
        var request = new
        {
            email,
            subject,
            body,
            from,
            replyTo,
            isHtml
        };

        var response = await client.PostAsJsonAsync("clients/api/email/sendQueue", request);
        response.EnsureSuccessStatusCode();
    }
}
