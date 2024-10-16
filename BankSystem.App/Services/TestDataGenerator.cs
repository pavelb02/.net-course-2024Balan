using BankSystem.App.Exeptions;
using BankSystem.Domain.Models;
using Bogus;
using Bogus.DataSets;
using Currency = BankSystem.Domain.Models.Currency;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    public List<Client> GenerateClientsBankList(int count)
    {
        Faker<Client> faker = new Faker<Client>("ru")
            .RuleFor(x => x.ClientId, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Name, (faker, _) => faker.Person.FirstName)
            .RuleFor(x => x.Surname, (faker, _) => faker.Person.LastName)
            .RuleFor(x => x.NumPassport, (faker, _) => faker.Random.Int(100000, 999999).ToString())
            .RuleFor(x => x.Phone, (faker, _) => faker.Person.Phone)
            .RuleFor(x => x.Balance, (faker, _) => faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.DateBirthday, (faker, _) => 
                DateTime.SpecifyKind(faker.Date.Between(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-18)), DateTimeKind.Utc))
            .RuleFor(x => x.Age, (faker, client) => CalculateAge(client.DateBirthday));

        return faker.Generate(count);
    }
    
    public Dictionary<string, Client> GenerateClientsBankDictionary(List<Client> clientsList)
    {
        var clientsDictionary = clientsList.ToDictionary(client => client.Phone);
        return clientsDictionary;
    }
    public Dictionary<Client, Account> GenerateClientsBankDictionaryAccount(List<Client> clientsList, Currency[] currencies)
    {
        var faker = new Faker<Account>("ru")
            .RuleFor(x => x.Amount, faker => faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.Currency,faker => faker.PickRandom(currencies));
        var accounts = faker.Generate(clientsList.Count).ToArray();
        var clientsDictionaryAccount = new Dictionary<Client, Account>();
        for (var i = 0; i < clientsList.Count; i++)
        {
            clientsDictionaryAccount.Add(clientsList[i], accounts[i]);
        }
        return clientsDictionaryAccount;
    }

    public Account[] GenerateAccountsArray(int count, string currencyCode)
    {
        Currency[] currencies =
        {
            new("USD", "Dollar USA", "$", 16.3m),
            new("EUR", "Euro", "€", 18.6m),
            new("RUP", "Russian ruble", "₽", 0.185m)
        };

        Currency selectedCurrency;
        if (currencies.Any(c => c.Code == currencyCode))
        {
            selectedCurrency = currencies.First(c => c.Code == currencyCode);
        }
        else
        {
            selectedCurrency = currencies.First(c => c.Code == "USD");
        }        var faker = new Faker<Account>("ru")
            .RuleFor(x => x.Amount, faker => faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.Currency,_  => selectedCurrency);
        return faker.Generate(count).ToArray();
    }
    public Dictionary<Client, Account[]> GenerateClientsBankDictionaryMultiAccount(List<Client> clientsList)
    {
        var accounts = GenerateAccountsArray(1, "USD");
        var clientsDictionaryMultiAccount = new Dictionary<Client, Account[]>();
        for (var i = 0; i < clientsList.Count; i++)
        {
            clientsDictionaryMultiAccount.Add(clientsList[i], accounts);
        }
        return clientsDictionaryMultiAccount;
    }
    public List<Employee> GenerateEmployeesBankList(int count, string[] positions)
    {
        Faker<Employee> faker = new Faker<Employee>("ru")
            .RuleFor(x => x.Name, (faker, _) => faker.Person.FirstName)
            .RuleFor(x => x.Id, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Surname, (faker, _) => faker.Person.LastName)
            .RuleFor(x => x.NumPassport, (faker, _) => faker.Random.Int(100000, 999999).ToString())
            .RuleFor(x => x.Phone, (faker, _) => faker.Person.Phone)
            .RuleFor(x => x.Position, (faker, _) => faker.PickRandom(positions))
            .RuleFor(x => x.Salary, (faker, _) => (int)faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.StartDate, (faker, _) => 
                DateTime.SpecifyKind(faker.Date.Past(15), DateTimeKind.Utc))
            .RuleFor(x => x.DateBirthday,
                (faker, _) => 
                    DateTime.SpecifyKind(faker.Date.Between(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-18)), DateTimeKind.Utc))
            .RuleFor(x => x.Age, (faker, employee) => CalculateAge(employee.DateBirthday));

        return faker.Generate(count);
    }
    public int CalculateAge(DateTime dateBirthday)
    {
        int age = DateTime.Now.Year - dateBirthday.Year;
        
        if (DateTime.Now < dateBirthday.AddYears(age))
        {
            age--;
        }

        return age;
    }
}