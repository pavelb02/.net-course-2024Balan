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

    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetClient([FromRoute] Guid clientId)
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
    
    [HttpDelete("{clientId}")]
    public async Task<IActionResult> DeleteClient([FromRoute] Guid clientId)
    {
        var response = await _clientService.DeleteClientAsync(clientId);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateClient([FromRoute] Guid clientId, [FromBody] ClientDto clientDto)
    {
        var response = await _clientService.UpdateClientAsync(clientId ,clientDto);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> SearchClients([FromQuery] SearchRequest searchRequest)
    {
        var response = await _clientService.FilterClientsAsync(searchRequest);
        return Ok(response);
    }
    
}