using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IStorage<Employee, SearchRequest> _employeeStorage;
    private readonly IMapper _mapper;

    public EmployeeService(IStorage<Employee, SearchRequest> employeeStorage, IMapper mapper)
    {
        _employeeStorage = employeeStorage;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        var employee = await _employeeStorage.GetByIdAsync(employeeId, cancellationToken);
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task<Guid> AddEmployeeAsync(EmployeeDto employeeDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!await ValidateAddEmployeeAsync(employeeDto))
            {
                return Guid.Empty;
            }
            
            var employee = _mapper.Map<Employee>(employeeDto);
            var employeeId = await _employeeStorage.AddAsync(employee, cancellationToken);
            return employeeId;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении сотрудника: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<Guid> UpdateEmployeeAsync(Guid employeeId, EmployeeDto newEmployee, CancellationToken cancellationToken)
    {
        try
        {
            var updatedEmployeeId = Guid.Empty;
            if (await ValidateAddEmployeeAsync(newEmployee))
            {
                var employee = await _employeeStorage.GetByIdAsync(employeeId, cancellationToken);
            
                if (!string.IsNullOrWhiteSpace(newEmployee.Name))
                    employee.Name = newEmployee.Name;

                if (!string.IsNullOrWhiteSpace(newEmployee.Surname))
                    employee.Surname = newEmployee.Surname;

                if (!string.IsNullOrWhiteSpace(newEmployee.NumPassport))
                    employee.NumPassport = newEmployee.NumPassport;

                if (newEmployee.DateBirthday != default)
                    employee.DateBirthday = newEmployee.DateBirthday;

                if (!string.IsNullOrWhiteSpace(newEmployee.Phone))
                    employee.Phone = newEmployee.Phone;
                
                if (!string.IsNullOrWhiteSpace(newEmployee.Position))
                    employee.Position = newEmployee.Position;

                if (newEmployee.StartDate != default)
                    employee.StartDate = newEmployee.StartDate;

                if (newEmployee.Salary > 0)
                    employee.Salary = newEmployee.Salary;

                updatedEmployeeId = await _employeeStorage.UpdateAsync(_mapper.Map<Employee>(newEmployee), cancellationToken);
            }
            return updatedEmployeeId;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при обновлении сторудника: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }
    
    public async Task<Guid> DeleteEmployeeAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        var deletedId = await _employeeStorage.DeleteAsync(employeeId, cancellationToken);
        return deletedId;
    }
    
    public async Task<List<EmployeeDto>> FilterEmployeesAsync(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var filteredEmployees = await _employeeStorage.GetCollectionAsync(searchRequest, cancellationToken);
        var filteredEmployeesDto = _mapper.Map<List<EmployeeDto>>(filteredEmployees);
        return filteredEmployeesDto;
    }

    private static Task<bool> ValidateAddEmployeeAsync(EmployeeDto employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            throw new ArgumentException("Имя не может быть null, пустым или состоять только из пробелов.",
                nameof(employee.Name));
        }

        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            throw new ArgumentException("Фамиилия не может быть null, пустой или состоять только из пробелов.",
                nameof(employee.Surname));
        }

        if (string.IsNullOrWhiteSpace(employee.NumPassport))
        {
            throw new ArgumentException("Номер паспорта не может быть null, пустым или состоять только из пробелов.",
                nameof(employee.NumPassport));
        }

        var dateNow = DateTime.Now;
        int age = dateNow.Year - employee.DateBirthday.Year;
        if (dateNow.DayOfYear < employee.DateBirthday.DayOfYear)
        {
            age--;
        }

        if (age < 18)
        {
            throw new PersonAgeException(age);
        }

        if (age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст должен быть положительным.");
        }

        return Task.FromResult(true);
    }
}