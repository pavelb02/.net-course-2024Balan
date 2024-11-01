using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class EmployeeService
{
    private readonly IStorage<Employee, SearchRequest> _employeeStorage;

    public EmployeeService(IStorage<Employee, SearchRequest> employeeStorage)
    {
        _employeeStorage = employeeStorage;
    }

    public async Task<Employee> GetEmployeeAsync(Guid employeeId)
    {
        return await _employeeStorage.GetByIdAsync(employeeId);
    }

    public async Task AddEmployeesAsync(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            try
            {
                if (await ValidateAddEmployee(employee))
                    await _employeeStorage.AddAsync(employee);
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
    }

    public async Task UpdateEmployeeAsync(Employee newEmployee)
    {
        try
        {
            if (await ValidateAddEmployee(newEmployee))
            {
                await _employeeStorage.UpdateAsync(newEmployee);
            }
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

    public async Task<List<Employee>> FilterEmployeesAsync(SearchRequest searchRequest)
    {
        var filteredEmployees = await _employeeStorage.GetCollectionAsync(searchRequest);
        return filteredEmployees;
    }

    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        await _employeeStorage.DeleteAsync(employeeId);
    }

    private static Task<bool> ValidateAddEmployee(Employee employee)
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