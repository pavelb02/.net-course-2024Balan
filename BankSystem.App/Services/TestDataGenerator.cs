using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    public List<Client> GenerateClientsBankList(int count)
    {
        var clientsList = new List<Client>();
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var name = "Name_client" + i;
            var surname = "Surname_client" + i;
            var age = random.Next(18, 70);
            var numPassport = "NP" + random.Next(100000, 999999);
            var phone = "+7" + + random.Next(100000000, 999999999);
            var accountNumber = random.Next(10000, 99999).ToString() + random.Next(10000, 99999);
            var balance = random.Next(100, 10000) + random.Next(0, 100) / 100m;
            clientsList.Add(new Client(name, surname, numPassport, age, phone, accountNumber, balance));
        }
        return clientsList;
    }
    public Dictionary<string, Client> GenerateClientsBankDictionary(List<Client> clientsList)
    {
        var clientsDictionary = clientsList.ToDictionary(client => client.Phone);
        return clientsDictionary;
    }
    public List<Employee> GenerateEmployeesBankList(int count, string[] positions)
    {
        var employeesList = new List<Employee>();
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var name = "Name_employee" + i;
            var surname = "Surname_employee" + i;
            var numPassport = "NP" + random.Next(100000, 999999);
            var age = random.Next(18, 70);
            var phone = "+7" + + random.Next(100000000, 999999999);
            var position = positions[random.Next(positions.Length)];
            var startDate = DateTime.Now.AddYears(-random.Next(1, 10));
            var salary = random.Next(60, 300) * 50;
            employeesList.Add(new Employee(name, surname, numPassport, age, phone, position, startDate, salary));
        }
        return employeesList;
    }
}