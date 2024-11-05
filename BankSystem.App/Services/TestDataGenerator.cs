using BankSystem.App.Dto;
using BankSystem.App.Exeptions;
using BankSystem.Domain.Models;
using Bogus;
using Bogus.DataSets;
using Currency = BankSystem.Domain.Models.Currency;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    public List<ClientDto> GenerateClientsBankList(int count)
    {
        Faker<ClientDto> faker = new Faker<ClientDto>("en")
            .RuleFor(x => x.Id, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Name, (faker, _) => faker.Person.FirstName)
            .RuleFor(x => x.Surname, (faker, _) => faker.Person.LastName)
            .RuleFor(x => x.NumPassport, (faker, _) => faker.Random.Int(100000, 999999).ToString())
            .RuleFor(x => x.Phone, (faker, _) => faker.Person.Phone)
            .RuleFor(x => x.DateBirthday, (faker, _) => 
                DateTime.SpecifyKind(faker.Date.Between(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-18)).Date, DateTimeKind.Utc));

        return faker.Generate(count);
    }

    public Dictionary<string, ClientDto> GenerateClientsBankDictionary(List<ClientDto> clientsList)
    {
        var clientsDictionary = clientsList.ToDictionary(client => client.Phone);
        return clientsDictionary;
    }
    public Dictionary<ClientDto, AccountDto> GenerateClientsBankDictionaryAccount(List<ClientDto> clientsList, Currency[] currencies)
    {
        var faker = new Faker<AccountDto>("en")
            .RuleFor(x => x.Id, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Amount, faker => faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.Currency,faker => faker.PickRandom(currencies));
        var accounts = faker.Generate(clientsList.Count).ToArray();
        var clientsDictionaryAccount = new Dictionary<ClientDto, AccountDto>();
        for (var i = 0; i < clientsList.Count; i++)
        {
            clientsDictionaryAccount.Add(clientsList[i], accounts[i]);
        }
        return clientsDictionaryAccount;
    }

    public AccountDto[] GenerateAccountsArray(int count, string currencyCode = "USD")
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
        }        var faker = new Faker<AccountDto>("en")
            .RuleFor(x => x.Id, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Amount, faker => faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.Currency,_  => selectedCurrency);
        return faker.Generate(count).ToArray();
    }
    public Dictionary<ClientDto, AccountDto[]> GenerateClientsBankDictionaryMultiAccount(List<ClientDto> clientsList)
    {
        var accounts = GenerateAccountsArray(1, "USD");
        var clientsDictionaryMultiAccount = new Dictionary<ClientDto, AccountDto[]>();
        for (var i = 0; i < clientsList.Count; i++)
        {
            clientsDictionaryMultiAccount.Add(clientsList[i], accounts);
        }
        return clientsDictionaryMultiAccount;
    }
    public List<EmployeeDto> GenerateEmployeesBankList(int count, string[] positions)
    {
        Faker<EmployeeDto> faker = new Faker<EmployeeDto>("en")
            .RuleFor(x => x.Id, (faker, _) => faker.Random.Guid())
            .RuleFor(x => x.Name, (faker, _) => faker.Person.FirstName)
            .RuleFor(x => x.Surname, (faker, _) => faker.Person.LastName)
            .RuleFor(x => x.NumPassport, (faker, _) => faker.Random.Int(100000, 999999).ToString())
            .RuleFor(x => x.Phone, (faker, _) => faker.Person.Phone)
            .RuleFor(x => x.Position, (faker, _) => faker.PickRandom(positions))
            .RuleFor(x => x.Salary, (faker, _) => (int)faker.Finance.Amount(100, 10000))
            .RuleFor(x => x.StartDate, (faker, _) => DateTime.SpecifyKind(faker.Date.Past(15).Date, DateTimeKind.Utc))
            .RuleFor(x => x.DateBirthday, (faker, _) =>
                DateTime.SpecifyKind(faker.Date.Between(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-18)).Date, DateTimeKind.Utc));

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