using fidelappback.Database;
using fidelappback.Models;
using fidelappback.Requetes.User.Request;
using fidelappback.Requetes.User.Response;
using fidelappback.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.controllers;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var userNewGuid = await _userService.Login(request.Email, request.Password);
        if (userNewGuid == null)
        {
            return NotFound(new LoginResponse());
        }
        return Ok(new LoginResponse { Guid = (Guid)userNewGuid });
    }

    // create a user, the user need to login after this step
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _userService.RegisterUser(request);
        if(response.IsEmailAlreadyUsed || response.IsPhoneNumberAlreadyUsed)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    // update user 
    [HttpPost("updatepassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {
        var response = await _userService.UpdatePassword(request);

        if (response.IsLoginCorrect && !response.IsPasswordToWeak)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}