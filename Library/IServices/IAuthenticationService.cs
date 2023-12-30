using Library.Models.DTOs.Request;

namespace Library.IServices;

public interface IAuthenticationService
{
    Task<string?> LoginAsync(LoginRequest loginRequest);
    Task<string?> RegisterAsync(RegisterRequest registerRequest);
    Task LogoutAsync();
}