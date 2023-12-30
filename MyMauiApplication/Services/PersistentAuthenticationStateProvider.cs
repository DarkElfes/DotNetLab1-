using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyMauiApplication.Services;

public class PersistentAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> defaultAuthenticationStateTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private Task<AuthenticationState> authenticationStateTask = defaultAuthenticationStateTask;
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await SecureStorage.Default.GetAsync("JWT_KEY") is string token)
        {
            JwtSecurityToken jwt = new(token);

            authenticationStateTask = Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims,
                    authenticationType: nameof(AuthenticationStateProvider)))));
        }
        else
            authenticationStateTask = defaultAuthenticationStateTask;

        NotifyAuthenticationStateChanged(authenticationStateTask);

        return authenticationStateTask.Result;
    }
}
