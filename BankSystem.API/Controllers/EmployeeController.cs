using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployee([FromRoute] Guid employeeId, CancellationToken cancellationToken)
    {
        var response = await _employeeService.GetEmployeeAsync(employeeId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto, CancellationToken cancellationToken)
    {
        var response = await _employeeService.AddEmployeeAsync(employeeDto, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId, CancellationToken cancellationToken)
    {
        var response = await _employeeService.DeleteEmployeeAsync(employeeId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromRoute] Guid employeeId, [FromBody] EmployeeDto employeeDto, CancellationToken cancellationToken)
    {
        var response = await _employeeService.UpdateEmployeeAsync(employeeId ,employeeDto, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> SearchEmployees([FromQuery] SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var response = await _employeeService.FilterEmployeesAsync(searchRequest, cancellationToken);
        return Ok(response);
    }
    
}