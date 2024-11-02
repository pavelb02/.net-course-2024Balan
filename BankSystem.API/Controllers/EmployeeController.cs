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

    [HttpGet]
    public async Task<IActionResult> GetEmployee([FromQuery] Guid employeeId)
    {
        var response = await _employeeService.GetEmployeeAsync(employeeId);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
    {
        var response = await _employeeService.AddEmployeeAsync(employeeDto);
        return Ok(response);
    }
    
    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId)
    {
        var response = await _employeeService.DeleteEmployeeAsync(employeeId);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] Guid employeeId, EmployeeDto employeeDto)
    {
        var response = await _employeeService.UpdateEmployeeAsync(employeeId ,employeeDto);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> SearchEmployees([FromQuery] SearchRequest searchRequest)
    {
        var response = await _employeeService.FilterEmployeesAsync(searchRequest);
        return Ok(response);
    }
    
}