using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage
{
    private Dictionary<Client, Account[]> Clients { get; set; }

    public ClientStorage()
    {
        Clients = new Dictionary<Client, Account[]>();
    }
    public Dictionary<Client, Account[]> GetAllClients()
    {
        return new Dictionary<Client, Account[]>(Clients);
    }
    public void AddClientToCollection(Dictionary<Client, Account[]> clients)
    {
        foreach (var client in clients)
        {
            Clients.Add(client.Key, client.Value);
        }
    }
    public Client? SearchYoungClient()
    {
        return Clients.MinBy(c => c.Key.Age).Key;
    }
    public Client? SearchOldClient()
    {
        return Clients.MaxBy(c => c.Key.Age).Key;
    }
    public int? SearchAverageAgeClient()
    {
        return (int)Clients.Average(c => c.Key.Age);
    }
}


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