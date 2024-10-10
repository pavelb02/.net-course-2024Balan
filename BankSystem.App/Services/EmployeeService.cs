using System.Collections.Specialized;
using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class EmployeeService
{
    private readonly IEmployeeStorage _employeeStorage;

    public EmployeeService(IEmployeeStorage employeeStorage)
    {
        _employeeStorage = employeeStorage;
    }

    public void AddEmployee(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            try
            {
                if (ValidateAddEmployee(employee))
                    _employeeStorage.Add(employee);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }
    }
    public List<Employee> FilterEmployees(SearchRequest searchRequest)
    {
        var filteredEmployees = _employeeStorage.Get(searchRequest);
        return filteredEmployees;
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