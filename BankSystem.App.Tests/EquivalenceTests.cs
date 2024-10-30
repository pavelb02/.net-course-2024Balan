using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    TestDataGenerator testDataGenerator = new();
    private ClientStorage _clientStorage;
    private ClientService _clientService;
    private CurrencyService _currencyService;
    private EmployeeStorage _employeeStorage;
    private CurrencyStorage _currencyStorage;
    private EmployeeService _employeeService;

    public EquivalenceTests()
    {
        _currencyStorage = new CurrencyStorage();
        _currencyService = new CurrencyService(_currencyStorage);
        _clientStorage = new ClientStorage();
        _clientService = new ClientService(_clientStorage, _currencyService);
        _employeeStorage = new EmployeeStorage();
        _employeeService = new EmployeeService(_employeeStorage);
    }

    string[] positions =
    {
        "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor",
        "IT specialist"
    };

    Currency[] currencies =
    {
        new("USD", "Dollar USA", "$", 16.3m),
        new("EUR", "Euro", "€", 18.6m),
        new("RUP", "Russian ruble", "₽", 0.185m)
    };

    private const string _defaultCurrencyCode = "USD";

    [Fact]
    public void GetHashCodeNecessityPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryAccount(clientsBankList, currencies);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new Client(client.Name, client.Surname, client.NumPassport, client.Phone,
            client.DateBirthday);
        //Act
        Account result = clientsBankDictionaryAccount[newClient];
        //Assert
        Assert.Equal(result, clientsBankDictionaryAccount[client]);
    }

    [Fact]
    public void GetHashCodeNecessityPositiveMultiAccountTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new Client(client.Name, client.Surname, client.NumPassport, client.Phone, client.DateBirthday);
        //Act
        Account[] result = clientsBankDictionaryAccount[newClient];
        //Assert
        Assert.Equal(result, clientsBankDictionaryAccount[client]);
    }

    [Fact]
    public void GetHashCodeNecessityPositiveListTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        var employee = employeesBankList.First();
        var newEmployee = new Employee(employee.Name, employee.Surname, employee.NumPassport,
            employee.Phone,
            employee.Position, employee.StartDate, employee.Salary, employee.DateBirthday);
        //Act
        bool result = employeesBankList.Contains(newEmployee);
        //Assert
        Assert.Equal(result, employeesBankList.Contains(employeesBankList.First()));
    }

    [Fact]
    public async Task AddClientAsyncPositiveListTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(13);
        foreach (var client in clientsBankList)
        {
            await _clientService.AddClientAsync(client, _defaultCurrencyCode);
        }

        //Act
        var clientOne = await _clientStorage.GetCollectionAsync(new SearchRequest{NumPassport = clientsBankList.First().NumPassport});
        //Assert
        Assert.Equal(clientsBankList.First(), clientOne.First());
    }

    [Fact]
    public async Task DeleteAllClientsAsyncPositiveListTest()
    {
        //Arrange
        var clientsBankList = await _clientService.FilterClientsAsync(new SearchRequest ());
        //Act
        foreach (var client in clientsBankList)
        {
            await _clientService.DeleteClientAsync(client.Id);
        }
        
        //Assert
        var clientsBankListNow = await _clientService.FilterClientsAsync(new SearchRequest ());
        Assert.Equal(0, clientsBankListNow.Count);
    }

    [Fact]
    public async Task AddClientAsyncNegativeListTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        clientsBankList.First().Name = "";
        //Act
        await _clientService.AddClientAsync(clientsBankList.First(), _defaultCurrencyCode);
        //Assert
        await Assert.ThrowsAsync<Exception>(async () =>
            await _clientService.AddClientAsync(clientsBankList.First(), _defaultCurrencyCode));

    }

    [Fact]
    public async Task AddAccountAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        await _clientService.AddClientAsync(clientsBankList.First(), _defaultCurrencyCode);
        await _clientService.AddAccountAsync(clientsBankList.First().Id, "USD");
        //Act
        var clients = await _clientService.GetClientAsync(clientsBankList.First().Id);
        var result = clients.AccountsClient.Count;
        //Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        await _clientService.AddClientAsync(clientsBankList.First(), _defaultCurrencyCode);
        var newClient = new Client(clientsBankList.First().Id)
        {
            Name = "Pavlik",
            Surname = "Balan",
            Phone = "+37368523915",
            NumPassport = "545464546"
        };
        await _clientService.UpdateClientAsync(clientsBankList.First().Id, newClient);
        //Act
        var result = await _clientService.GetClientAsync(clientsBankList.First().Id);
        //Assert
        Assert.Equal(result, newClient);
    }

    [Fact]
    public async Task FilterClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        await _clientService.AddClientAsync(clientsBankList.First(), _defaultCurrencyCode);
        var searchRequest = new SearchRequest { NumPassport = clientsBankList[0].NumPassport };
        //Act
        var filteredClients = await _clientService.FilterClientsAsync(searchRequest);
        // Assert
        Assert.Single(filteredClients);
        Assert.Equal(clientsBankList[0].NumPassport, filteredClients.First().NumPassport);
    }
    
    [Fact]
    public async Task DebitAsyncPositiveTest()
    {
        //Arrange & Act
        var pageSize = 5;
        var iterationCount = 2;
        var withdrawalAmount = 100;
        var currencyCode = "USD";

        var currencyId = await _currencyService.GetCurrencyAsync(currencyCode);
        var clientBefore = await _clientService.FilterClientsAsync(new SearchRequest { PageNumber = 1, PageSize = 1 });
        var amountBefore = clientBefore.First().AccountsClient.FirstOrDefault(a => a.CurrencyId == currencyId)!.Amount;
        
        for (int i = 0; i < iterationCount; i++)
        {
            var pageNumber = 1;
            var flag = true;
            while (flag)
            {
                var clients = await _clientStorage.GetCollectionAsync(new SearchRequest
                    { PageSize = pageSize, PageNumber = pageNumber });
                if (clients.Count < pageSize)
                    flag = false;

                foreach (var client in clients)
                {
                    var task = _clientService.Debit(new WithdrawalRequest
                    {
                        ClientId = client.Id,
                        WithdrawalAmount = withdrawalAmount,
                        CurrencyCode = currencyCode
                    });

                    await task; 
                }

                pageNumber++;
            }
        }

        // Assert
        var clientAfter = await _clientService.FilterClientsAsync(new SearchRequest { PageNumber = 1, PageSize = 1 });
        var amountAfter = clientAfter.First().AccountsClient.FirstOrDefault(a => a.CurrencyId == currencyId);
        Assert.Equal(amountBefore - withdrawalAmount - withdrawalAmount, amountAfter.Amount);
    }

    [Fact]
    public async Task AddEmployeeAsyncPositiveListTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(3, positions);
        await _employeeService.AddEmployeesAsync(employeesBankList);
        //Act
        var employee = await _employeeStorage.GetCollectionAsync(new SearchRequest());
        //Assert
        Assert.Equal(employee, employeesBankList);
    }

    [Fact]
    public async Task FilterEmployeeAsyncPositiveTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        await _employeeService.AddEmployeesAsync(employeesBankList);
        var searchRequest = new SearchRequest { Name = employeesBankList[0].Name };
        //Act
        var filteredEmployees = await _employeeService.FilterEmployeesAsync(searchRequest);
        //Assert
        Assert.Equal(employeesBankList[0].NumPassport, filteredEmployees.First().NumPassport);
    }

    [Fact]
    public async Task DeleteEmployeeAsyncPositiveListTest()
    {
        //Arrange
        var employeesBankList = await _employeeService.FilterEmployeesAsync(new SearchRequest { });
        //Act
        await _employeeService.DeleteEmployeeAsync(employeesBankList.First().Id);
        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _employeeStorage.GetByIdAsync(employeesBankList.First().Id));
    }
}