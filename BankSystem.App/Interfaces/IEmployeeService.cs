using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeAsync(Guid employeeId);
    Task<Guid> AddEmployeesAsync(EmployeeDto employee);
    Task<Guid> UpdateEmployeeAsync(Guid employeeId, EmployeeDto newEmployee);
    Task<Guid> DeleteEmployeeAsync(Guid employeeId);
    Task<List<EmployeeDto>> FilterEmployeesAsync(SearchRequest searchRequest);
}