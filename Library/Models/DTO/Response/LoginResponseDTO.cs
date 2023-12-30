using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace Library.Models.DTO.Response;

public class LoginResponseDTO
{
    public User User { get; set; }
    public string Token { get; set; }

    public void Deconstruct(out User user, out string token)
        => (user, token) = (User, Token);
}
