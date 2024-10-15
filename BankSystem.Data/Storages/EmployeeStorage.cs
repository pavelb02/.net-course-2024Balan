using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class EmployeeStorage : IStorage<Employee, SearchRequest>
{

    private BankSystemDbContext _dbContext;
    public EmployeeStorage()
    {
        _dbContext = new BankSystemDbContext();
    }
    
    public void Add(Employee employee)
    {
        if (_dbContext.Employees.Any(e => e.Id == employee.Id))
            return;

        _dbContext.Employees.Add(employee);
    }

    public Employee GetById(Guid employeeId)
    {
        return _dbContext.Employees.FirstOrDefault(e => e.Id == employeeId);
    }

    public List<Employee> GetCollection(SearchRequest searchRequest)
    {
        IEnumerable<Employee> employees = _dbContext.Employees;
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

    public void Update(Guid employeeId, Employee employee)
    {
        var updateEmployee = _dbContext.Employees.FirstOrDefault(e => e.Id == employeeId);

        if (updateEmployee == null) return;
        
        updateEmployee.Name = employee.Name;
        updateEmployee.Surname = employee.Surname;
        updateEmployee.Phone = employee.Phone;
        updateEmployee.NumPassport = employee.NumPassport;
        updateEmployee.DateBirthday = employee.DateBirthday;
        updateEmployee.Position = employee.Position;
        updateEmployee.Salary = employee.Salary;
        
        _dbContext.SaveChanges();
    }

    public void Delete(Guid employeeId)
    {
        var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == employeeId);
        if (employee == null) return;
        _dbContext.Employees.Remove(employee);
        
        _dbContext.SaveChanges();
    }
    
    public Employee? SearchYoungEmployee()
    {
        return _dbContext.Employees.MinBy(c => c.Age);
    }
    public Employee? SearchOldEmployee()
    {
        return _dbContext.Employees.MaxBy(c => c.Age);
    }
    public int SearchAverageAgeEmployee()
    {
        return (int)_dbContext.Employees.Average(c => c.Age);
    }
}