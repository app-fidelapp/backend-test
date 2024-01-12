using fidelappback.Database;
using fidelappback.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.controllers;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{

    // get db context
    private readonly FidelappDbContext _context;
    // get db context from constructor
    public UserController(FidelappDbContext context)
    {
        _context = context;
    }

    // connection endpoint, returns a jwt token
    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        // check if user exists
        var userExists = _context.User.Any(u => u.PhoneNumber == user.PhoneNumber && u.Password == user.Password);
        if (!userExists)
        {
            return BadRequest("User doesn't exist");
        }
        // get user from db
        var userFromDb = _context.User.FirstOrDefault(u => u.PhoneNumber == user.PhoneNumber && u.Password == user.Password);
        // update last connection
        userFromDb.LastConnection = DateTime.Now;
        // generate a new guid
        userFromDb.Guid = Guid.NewGuid().ToString();
        _context.SaveChanges();
        // return jwt token
        return Ok(userFromDb.Guid);
    }

    // create a user
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // check if user already exists
        var userExists = await _context.User.AnyAsync(u => u.PhoneNumber == user.PhoneNumber || u.Email == user.Email);
        if (userExists)
        {
            return BadRequest("User already exists");
        }
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }
}