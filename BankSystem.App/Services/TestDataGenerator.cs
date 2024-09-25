using BankSystem.Domain.Models;
using Bogus;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    public List<Client> GenerateClientsBankList(int count)
    {
        Faker<Client> faker = new Faker<Client>("ru")
            .RuleFor(x => x.ClientId, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Name, (faker, _) => faker.Person.FirstName)
            .RuleFor(x => x.Surname, (faker, _) => faker.Person.LastName)
            .RuleFor(x => x.Age, (faker, _) => faker.Random.Int(21,50))
            .RuleFor(x => x.NumPassport, (faker, _) => faker.Random.Int(100000, 999999).ToString())
            .RuleFor(x => x.Phone, (faker, _) => faker.Person.Phone)
            .RuleFor(x => x.AccountNumber, (faker, _) => faker.Finance.Account(10))
            .RuleFor(x => x.Balance, (faker, _) => faker.Finance.Amount(100, 10000));
        
        return faker.Generate(1000);
    }
    public Dictionary<string, Client> GenerateClientsBankDictionary(List<Client> clientsList)
    {
        var clientsDictionary = clientsList.ToDictionary(client => client.Phone);
        return clientsDictionary;
    }
    public List<Employee> GenerateEmployeesBankList(int count, string[] positions)
    {
        Faker<Employee> faker = new Faker<Employee>("ru")
            .RuleFor(x => x.Name, (faker, _) => faker.Person.FirstName)
            .RuleFor(x => x.EmployeeId, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Surname, (faker, _) => faker.Person.LastName)
            .RuleFor(x => x.Age, (faker, _) => faker.Random.Int(21,50))
            .RuleFor(x => x.NumPassport, (faker, _) => faker.Random.Int(100000, 999999).ToString())
            .RuleFor(x => x.Phone, (faker, _) => faker.Person.Phone)
            .RuleFor(x => x.Position, (faker, _) => faker.PickRandom(positions))
            .RuleFor(x => x.Salary, (faker, _) => (int)faker.Finance.Amount(100, 10000))
            .RuleFor(x=>x.StartDate, (faker, _)=> faker.Date.Past(15));

        return faker.Generate(1000);
        
    }
}