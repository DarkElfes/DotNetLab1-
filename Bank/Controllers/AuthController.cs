using Library.Models.DTOs.Request;
using Library.Models.DTOs.Response;
using Bank.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserManager _userManager;

    public AuthController(IUserManager userManager) => _userManager = userManager;

    [HttpPost]
    [Route("Register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult RegisterAsync([FromBody] RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Model has incorrect data");
        else if (!_userManager.IsUniqueUser(registerRequest.Email))
            return Conflict("User already exist");


        try
        {
            AuthResponse? authResponse = _userManager.RegisterAsync(registerRequest);

            if (authResponse is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(authResponse);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Some error on server, try again later");
        }
    }


    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        AuthResponse? authResponse = _userManager.Login(loginRequest);

        if (!ModelState.IsValid)
            return BadRequest("Model has an incorrect data");
        else if (authResponse is null)
            return BadRequest("Email or password is incorrect");

        return Ok(authResponse);
    }
}
