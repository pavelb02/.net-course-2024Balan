using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class EmployeeStorage
{
    private List<Employee> Employees { get; set; }
    public EmployeeStorage()
    {
        Employees = new List<Employee>();
    }
    public List<Employee> GetAllEmployees()
    {
        return new List<Employee>(Employees);
    }
    public void AddEmployeeToCollection(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            Employees.Add(employee);
        }
    }
    public void UpdateCollection(List<Employee> employees)
    {
        Employees.Clear();
        foreach (var employee in employees)
        {
            Employees.Add(employee);
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