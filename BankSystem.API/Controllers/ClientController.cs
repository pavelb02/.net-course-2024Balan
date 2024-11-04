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
    public async Task<IActionResult> GetClient([FromRoute] Guid clientId, CancellationToken cancellationToken)
    {
        var response = await _clientService.GetClientAsync(clientId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto, CancellationToken cancellationToken)
    {
        var response = await _clientService.AddClientAsync(clientDto, "USD", cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{clientId}")]
    public async Task<IActionResult> DeleteClient([FromRoute] Guid clientId, CancellationToken cancellationToken)
    {
        var response = await _clientService.DeleteClientAsync(clientId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateClient([FromRoute] Guid clientId, [FromBody] ClientDto clientDto, CancellationToken cancellationToken)
    {
        var response = await _clientService.UpdateClientAsync(clientId ,clientDto, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> SearchClients([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var response = await _clientService.FilterClientsAsync(searchRequest, cancellationToken);
        return Ok(response);
    }
    
}