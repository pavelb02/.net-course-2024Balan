using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    TestDataGenerator testDataGenerator = new ();
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
    public void AddClientPositiveListTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(3);
        foreach (var client in clientsBankList)
        {
            _clientService.AddClients(client, _defaultCurrencyCode);
        }
        //Act
        var clients = _clientStorage.GetCollection(new SearchRequest());
        //Assert
        Assert.Equal(clients, clientsBankList);
    }
    [Fact]
    public void DeleteClientPositiveListTest()
    {
        //Arrange
        var clientsBankList = _clientService.FilterClients(new SearchRequest {});
        //Act
        _clientService.DeleteClient(clientsBankList.First().Id);
        //Assert
        Assert.Throws<ArgumentException>(() => _clientStorage.GetById(clientsBankList.First().Id));
    }
    [Fact]
    public void AddClientNegativeListTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        clientsBankList.First().Name = "";
        //Act
        _clientService.AddClients(clientsBankList.First(), _defaultCurrencyCode);     
        //Assert
        Assert.Throws<Exception>(() => _clientService.AddClients(clientsBankList.First(), _defaultCurrencyCode));
        
    }

    [Fact]
    public void AddAccountPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        _clientService.AddClients(clientsBankList.First(),_defaultCurrencyCode);
        _clientService.AddAccount(clientsBankList.First().Id, "USD");
        //Act
        var result = _clientService.GetClient(clientsBankList.First().Id).AccountsClient.Count;
        //Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void UpdateClientPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        _clientService.AddClients(clientsBankList.First(),_defaultCurrencyCode);
        var newClient = new Client(clientsBankList.First().Id);
        newClient.Name = "Pavlik";
        newClient.Surname = "Balan";
        newClient.Phone = "+37368523915";
        _clientService.UpdateClient(clientsBankList.First().Id, newClient);
        //Act
        var result = _clientService.GetClient(clientsBankList.First().Id);
        //Assert
        Assert.Equal(result, newClient);
    }

    [Fact]
    public void FilterClientPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        _clientService.AddClients(clientsBankList.First(), _defaultCurrencyCode);
        var searchRequest = new SearchRequest { NumPassport = clientsBankList[0].NumPassport };
        //Act
        var filteredClients =  _clientService.FilterClients(searchRequest);
        // Assert
        Assert.Single(filteredClients);
        Assert.Equal(clientsBankList[0].NumPassport, filteredClients.First().NumPassport); 
    }

    [Fact]
    public void AddEmployeePositiveListTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        _employeeService.AddEmployees(employeesBankList);
        //Act
        var employee = _employeeStorage.GetCollection(new SearchRequest());
        //Assert
        Assert.Equal(employee, employeesBankList);
    }

    [Fact]
    public void FilterEmployeePositiveTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        _employeeService.AddEmployees(employeesBankList);
        var searchRequest = new SearchRequest { Name = employeesBankList[0].Name };
        //Act
        var filteredEmployees = _employeeService.FilterEmployees(searchRequest);
        //Assert
        Assert.Equal(employeesBankList[0].NumPassport, filteredEmployees.First().NumPassport);
    }
}