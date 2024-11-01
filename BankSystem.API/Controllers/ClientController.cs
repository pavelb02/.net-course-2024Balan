using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClient([FromQuery] Guid clientId)
    {
        var response = await _clientService.GetClientAsync(clientId);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto)
    {
        var response = await _clientService.AddClientAsync(clientDto, "USD");
        return Ok(response);
    }
    
}