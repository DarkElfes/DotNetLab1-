using Library.IServices;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace MyMauiApplication.Handlers;

public class AuthenticationHandler(IConfiguration configuration) : DelegatingHandler
{
    private readonly IConfiguration _configuration = configuration;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? jwt = await SecureStorage.Default.GetAsync("JWT_KEY");
        bool isToServer = request.RequestUri?.AbsoluteUri.StartsWith(_configuration["ServerUrl"] ?? "") ?? false;

        if (isToServer && !string.IsNullOrEmpty(jwt))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        return await base.SendAsync(request, cancellationToken);
    }
}
