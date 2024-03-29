using fidelappback.Requetes.Client.Request;
using fidelappback.Services;
using Microsoft.AspNetCore.Mvc;

namespace fidelappback.controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IUserService _userService;
    public ClientController(IClientService clientService, IUserService userService)
    {
        _clientService = clientService;
        _userService = userService;
    }

    // register client
    [HttpPost("newvisitclient")]
    public async Task<IActionResult> NewVisitClientAsync(NewVisitClientRequest request)
    {   
        var user = await _userService.IsAuthorizedAsync(request);
        if (user == null)
        {
            return Unauthorized();
        }

        var response = await _clientService.RegisterClientAsync(request, user);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    // confirm visit client
    [HttpPost("confirmvisitclient")]
    public async Task<IActionResult> ConfirmVisitClientAsync(ConfirmVisitClientRequest request)
    {
        var user = await _userService.IsAuthorizedAsync(request);
        if (user == null)
        {
            return Unauthorized();
        }

        var response = await _clientService.ConfirmVisitClientAsync(request, user);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}