using System.ComponentModel.DataAnnotations;

namespace Library.Models.DTOs.Request;

public class RegisterRequest : LoginRequest
{
    [Required]
    [MaxLength(20, ErrorMessage = "First name must be less than 20 characters")]
    public string Name { get; set; } = string.Empty;
}

