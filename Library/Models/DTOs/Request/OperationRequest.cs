using System.ComponentModel.DataAnnotations;

namespace Library.Models.DTOs.Request;

public class OperationRequest
{
    [Range(1, 100000, ErrorMessage = "Value must be equal or greater than 1 and less than 100000")]
    public decimal Amount { get; set; }
}
