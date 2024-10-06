using System.Collections.Specialized;
using BankSystem.App.Exeptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class EmployeeService
{
    private EmployeeStorage _employeeStorage;

    public EmployeeService(EmployeeStorage employeeStorage)
    {
        _employeeStorage = employeeStorage;
    }

    public void AddEmployee(List<Employee> employees)
    {
        var employeesCorrect = new List<Employee>();
        foreach (var employee in employees)
        {
            try
            {
                if (ValidateAddEmployee(employee))
                    employeesCorrect.Add(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }

        _employeeStorage.AddEmployeeToCollection(employeesCorrect);
    }
    public List<Employee> FilterEmployees(SearchRequest searchRequest)
    {
        var employees = _employeeStorage.GetAllEmployees();
        if (!string.IsNullOrWhiteSpace(searchRequest.Name))
        {
            employees = employees.Where(e => e.Name == searchRequest.Name).ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Surname))
        {
            employees = employees.Where(e => e.Surname == searchRequest.Surname).ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Phone))
        {
            employees = employees.Where(e => e.Phone == searchRequest.Phone).ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.NumPassport))
        {
            employees = employees.Where(e => e.NumPassport == searchRequest.NumPassport).ToList();
        }

        if (searchRequest.DateStart != null && searchRequest.DateEnd != null &&
            searchRequest.DateStart <= searchRequest.DateEnd)
        {
            employees = employees.Where(e => 
                e.DateBirthday >= searchRequest.DateStart && e.DateBirthday <= searchRequest.DateEnd).ToList(); 
        }
        return employees;
    }
    private bool ValidateAddEmployee(Employee employee)
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