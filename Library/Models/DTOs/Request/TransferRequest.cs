using System.ComponentModel.DataAnnotations;

namespace Library.Models.DTOs.Request;

public class TransferRequest : OperationRequest
{
    [Required]
    [RegularExpression(@"^ ?(\d{4} ?){4}$", ErrorMessage ="Inccorect card number, it's must contain 16 digits")]
    public string To { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "Reason can not be more than 100 characters")]
    public string? Reason { get; set; }

}
