using Bank.Services.IServices;
using Library.Models.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers;

[Authorize]
[Route("api/[controller]")]

public class CardController(ICardService cardService) : ControllerBase
{
    private readonly ICardService _cardService = cardService;

    [HttpGet]
    public IActionResult Get()
        => Ok(_cardService.GetCard());

    [HttpPut]
    [Route("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest transferRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Incorrect data");
        else
            return Ok(_cardService.Transfer(transferRequest));
    }

    [HttpPut]
    [Route("Deposit")]
    public IActionResult Deposit([FromBody] OperationRequest operationRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Incorrect date");
        else
            return Ok(_cardService.Deposit(operationRequest));
    }

    [HttpPut]
    [Route("Withdraw")]
    public IActionResult Withdraw([FromBody] OperationRequest operationRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Incorrect date");
        else
            return Ok(_cardService.Withdraw(operationRequest));
    }
}
