using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet("{currencyCode}")]
    public async Task<IActionResult> GetCurrencyId([FromRoute] string currencyCode, CancellationToken cancellationToken)
    {
        var response = await _currencyService.GetCurrencyAsync(currencyCode, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCurrency([FromBody] CurrencyDto currencyDto, CancellationToken cancellationToken)
    {
        var response = await _currencyService.AddCurrencyAsync(currencyDto, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{currencyCode}")]
    public async Task<IActionResult> DeleteCurrency([FromRoute] string currencyCode, CancellationToken cancellationToken)
    {
        var response = await _currencyService.DeleteCurrencyAsync(currencyCode, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> ConversionCurrency([FromQuery] ConversionCurrencyRequest conversionCurrencyRequest, CancellationToken cancellationToken)
    {
        var response = await _currencyService.CurrencyConversionAsync(conversionCurrencyRequest, cancellationToken);
        return Ok(response);
    }
    
}