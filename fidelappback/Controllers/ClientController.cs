using fidelappback.Services;
using Microsoft.AspNetCore.Mvc;

namespace fidelappback.controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }
}