using Library.Models.DTOs.Request;
using Library.Models.DTOs.Response;
using System.Net.Http.Json;
using Library.IServices;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyMauiApplication.Services;

public class AuthenticationService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authStateProvider) : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

    private const string JWT_KEY = nameof(JWT_KEY);

    public async Task LogoutAsync()
    {
        if (SecureStorage.Default.Remove(JWT_KEY))
            await _authStateProvider.GetAuthenticationStateAsync();
    }

    public async Task<string?> LoginAsync(LoginRequest loginRequest)
    {
        using HttpClient client = _httpClientFactory.CreateClient("ServerApi");
        return await ResponseHandlerAsync(await client.PostAsJsonAsync("api/auth/login", loginRequest));
    }

    public async Task<string?> RegisterAsync(RegisterRequest registerRequest)
    {
        using HttpClient client = _httpClientFactory.CreateClient("ServerApi");
        return await ResponseHandlerAsync(await client.PostAsJsonAsync("api/auth/register", registerRequest));
    }

    private async Task<string?> ResponseHandlerAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();

        AuthResponse? content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (content == null)
            throw new InvalidDataException();

        await SecureStorage.Default.SetAsync(JWT_KEY, content.JwtToken);
        await _authStateProvider.GetAuthenticationStateAsync();

        return null;
    }
}