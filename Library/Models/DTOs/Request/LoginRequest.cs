using System.ComponentModel.DataAnnotations;

namespace Library.Models.DTOs.Request;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password can not be more than 50 and less then 6 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
