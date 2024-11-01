using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace BankSystem.Data.Storages;

public class EmployeeStorage : IStorage<Employee, SearchRequest>
{

    private BankSystemDbContext _dbContext;
    public EmployeeStorage()
    {
        _dbContext = new BankSystemDbContext();
    }
    
    public async Task AddAsync(Employee employee)
    {
        if (await _dbContext.Employees.AnyAsync(e => e.Id == employee.Id))
        {
            throw new InvalidOperationException($"Сотрудник с ID {employee.Id} уже существует.");
        }

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Employee> GetByIdAsync(Guid employeeId)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null) throw new ArgumentException($"Сотрудник с Id {employeeId} не найден.");
        return employee;
    }

    public async Task<List<Employee>> GetCollectionAsync(SearchRequest searchRequest)
    {
        IQueryable<Employee> request =  _dbContext.Employees;
        if (!string.IsNullOrWhiteSpace(searchRequest.Name))
        {
            request = request.Where(e => e.Name == searchRequest.Name);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Surname))
        {
            request = request.Where(e => e.Surname == searchRequest.Surname);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Phone))
        {
            request = request.Where(e => e.Phone == searchRequest.Phone);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.NumPassport))
        {
            request = request.Where(e => e.NumPassport == searchRequest.NumPassport);
        }

        if (searchRequest.DateStart != null && searchRequest.DateEnd != null &&
            searchRequest.DateStart <= searchRequest.DateEnd)
        {
            request = request.Where(e => 
                e.DateBirthday >= searchRequest.DateStart && e.DateBirthday <= searchRequest.DateEnd); 
        }
        //var countRecords = request.Count();
        if (searchRequest.PageSize != 0 && searchRequest.PageNumber != 0)
        {
            return await request
                .OrderBy(c => c.Surname).ThenBy(c => c.Name)
                .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToListAsync();
        }

        return await request.ToListAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        _dbContext.Update(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid employeeId)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null) return;
        _dbContext.Employees.Remove(employee);
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Employee?> SearchYoungEmployee()
    {
        return await _dbContext.Employees.OrderBy(e => e.DateBirthday).FirstOrDefaultAsync();
    }
    public async Task<Employee?> SearchOldEmployee()
    {
        return await _dbContext.Employees.OrderByDescending(e => e.DateBirthday).FirstOrDefaultAsync();
    }
    public async Task<int> SearchAverageAgeEmployee()
    {
        var dateNow = DateTime.Now;
        return (int) await _dbContext.Employees.AverageAsync(e => dateNow.Year - e.DateBirthday.Year -
                                                                  (dateNow.DayOfYear < e.DateBirthday.DayOfYear ? 1 : 0));
    }
}