using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<Guid> AddEmployeeAsync(EmployeeDto employee, CancellationToken cancellationToken);
    Task<Guid> UpdateEmployeeAsync(Guid employeeId, EmployeeDto newEmployee, CancellationToken cancellationToken);
    Task<Guid> DeleteEmployeeAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<List<EmployeeDto>> FilterEmployeesAsync(SearchRequest searchRequest, CancellationToken cancellationToken);
}