using fidelappback.Requetes.User.Request;
using fidelappback.Requetes.User.Response;
using fidelappback.Services;
using Microsoft.AspNetCore.Mvc;

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
        var connectionString = await _userService.LoginAsync(request.Email, request.Password);
        if (connectionString == null)
        {
            return NotFound(new LoginResponse());
        }
        return Ok(new LoginResponse { ConnectionString = connectionString });
    }

    // create a user, the user need to login after this step
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _userService.RegisterUserAsync(request);
        if(response.IsEmailAlreadyUsed || response.IsPhoneNumberAlreadyUsed)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    // update user password
    [HttpPost("updatepassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {
        var response = await _userService.UpdatePasswordAsync(request);

        if (response.IsLoginCorrect && !response.IsPasswordToWeak)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    // update user data
    [HttpPost("updateuser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var user = await _userService.IsAuthorizedAsync(request);
        if (user == null)
        {
            return Unauthorized();
        }

        var response = await _userService.UpdateUserAsync(user, request);

        return Ok(response);
    }
}