using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class EmployeeStorage : IEmployeeStorage
{
    private List<Employee> Employees { get; set; }
    public EmployeeStorage()
    {
        Employees = new List<Employee>();
    }
    
    public void Add(Employee employee)
    {
        if (Employees.Any(e => e.EmployeeId == employee.EmployeeId))
        {
            throw new InvalidOperationException($"Сотрудник с ID {employee.EmployeeId} уже добавлен.");
        }

        Employees.Add(employee);
    }

    public List<Employee> Get(SearchRequest searchRequest)
    {
        IEnumerable<Employee> employees = Employees;
        if (!string.IsNullOrWhiteSpace(searchRequest.Name))
        {
            employees = employees.Where(e => e.Name == searchRequest.Name);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Surname))
        {
            employees = employees.Where(e => e.Surname == searchRequest.Surname);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Phone))
        {
            employees = employees.Where(e => e.Phone == searchRequest.Phone);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.NumPassport))
        {
            employees = employees.Where(e => e.NumPassport == searchRequest.NumPassport);
        }

        if (searchRequest.DateStart != null && searchRequest.DateEnd != null &&
            searchRequest.DateStart <= searchRequest.DateEnd)
        {
            employees = employees.Where(e => 
                e.DateBirthday >= searchRequest.DateStart && e.DateBirthday <= searchRequest.DateEnd); 
        }
        return employees.ToList();
    }

    public void Update(Employee employee)
    {
        var oldEmployee = Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
    
        if (oldEmployee != null)
        {
            oldEmployee.Name = employee.Name;
            oldEmployee.Surname = employee.Surname;
            oldEmployee.Phone = employee.Phone;
            oldEmployee.NumPassport = employee.NumPassport;
            oldEmployee.DateBirthday = employee.DateBirthday;
            oldEmployee.Position = employee.Position;
            oldEmployee.Salary = employee.Salary;
        }
        else
        {
            throw new EntityNotFoundException("Сотрудник не найден с данным ID: " + employee.EmployeeId);
        }
    }

    public void Delete(Employee employee)
    {
        if (Employees.Contains(employee))
        {
            Employees.Remove(employee);
        }
        else
        {
            throw new EntityNotFoundException("Сотрудник не найден.");
        }
    }
    public Employee? SearchYoungEmployee()
    {
        return Employees.MinBy(c => c.Age);
    }
    public Employee? SearchOldEmployee()
    {
        return Employees.MaxBy(c => c.Age);
    }
    public int? SearchAverageAgeEmployee()
    {
        return (int)Employees.Average(c => c.Age);
    }
}