using Library.Models.DTOs.Request;
using Library.Models.DTOs.Response;

namespace Bank.Services.IServices;

public interface IUserManager
{
    bool IsUniqueUser(string email);
    AuthResponse? RegisterAsync(RegisterRequest registerRequest);
    AuthResponse? Login(LoginRequest loginRequest);

}