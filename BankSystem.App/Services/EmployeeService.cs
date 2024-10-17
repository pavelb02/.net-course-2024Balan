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

    public Employee GetEmployee(Guid employeeId)
    {
        return _employeeStorage.GetById(employeeId);
    }

    public void AddEmployees(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            try
            {
                if (ValidateAddEmployee(employee))
                    _employeeStorage.Add(employee);
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

    public void UpdateEmployee(Guid employeeId, Employee newEmployee)
    {
        try
        {
            if (ValidateAddEmployee(newEmployee))
            {
                _employeeStorage.Update(employeeId, newEmployee);
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

    public List<Employee> FilterEmployees(SearchRequest searchRequest)
    {
        var filteredEmployees = _employeeStorage.GetCollection(searchRequest);
        return filteredEmployees;
    }

    public void DeleteEmployee(Guid employeeId)
    {
        _employeeStorage.Delete(employeeId);
    }
  
    private static bool ValidateAddEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            throw new ArgumentException("Имя не может быть null, пустым или состоять только из пробелов.", nameof(employee.Name));
        }

        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            throw new ArgumentException("Фамиилия не может быть null, пустой или состоять только из пробелов.", nameof(employee.Surname));
        }

        if (string.IsNullOrWhiteSpace(employee.NumPassport))
        {
            throw new ArgumentException("Номер паспорта не может быть null, пустым или состоять только из пробелов.", nameof(employee.NumPassport));
        }

        if (employee.Age < 18)
        {
            throw new PersonAgeException(employee.Age);
        }

        if (employee.Age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(employee.Age), "Возраст должен быть положительным.");
        }

        return true;
    }
}