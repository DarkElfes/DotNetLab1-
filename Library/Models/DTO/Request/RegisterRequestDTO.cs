using System.ComponentModel.DataAnnotations;

namespace Library.Models.DTO.Request;

public class RegisterRequestDTO : LoginRequestDTO
{
    [Required]
    [MaxLength(20, ErrorMessage = "First name must be less then 20 characters")]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(20, ErrorMessage = "Last name must be less then 20 characters")]
    public string LastName { get; set; }
}

public class LoginRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password can not be more then 50 and less then 6 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
