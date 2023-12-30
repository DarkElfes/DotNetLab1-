using Library.Models;
using Library.Models.DTOs.Response;
using Library.Models.DTOs.Request;
using AutoMapper;
using Bank.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bank.Services.IServices;

namespace Bank.Services;

public class UserManager(
    IMapper mapper,
    IConfiguration config,
    IUserRepository userRepo,
    ICardRepository cardRepo) : IUserManager
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly ICardRepository _cardRepo = cardRepo;

    private string secretKey = config.GetValue<string>("ApiSettings:SecretKey") ?? throw new ArgumentNullException("Secret ket not finded");

    public bool IsUniqueUser(string email)
        => _userRepo.GetByEmail(email) is null;

    public AuthResponse? RegisterAsync(RegisterRequest registerRequest)
    {
        User user = _mapper.Map<RegisterRequest, User>(registerRequest);
        user.Role = "user";

        Card card = new()
        {
            Number = (_cardRepo.GetAll().OrderByDescending(c => c.Id).FirstOrDefault()?.Id + 1 ?? 1).ToString().PadLeft(16, '0'),
        };
        user.CardId = _cardRepo.Create(card).Id;

        _userRepo.Create(user);

        return Login(new()
        {
            Email = user.Email,
            Password = user.Password
        });
    }

    public AuthResponse? Login(LoginRequest loginRequest)
    {
        User? user = _userRepo.GetByEmail(loginRequest.Email);

        if (user is null || user.Password != loginRequest.Password)
            return null;

        byte[] key = Encoding.ASCII.GetBytes(secretKey);

        List<Claim> authClaims = new()
        {
              new Claim(ClaimTypes.Name, user.Name),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim("card_id", user.CardId.ToString()),
        };

        JwtSecurityToken token = new(
            claims: authClaims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

        return new()
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
        };

    }
}
