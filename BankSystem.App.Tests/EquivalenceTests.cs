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
    private EmployeeStorage _employeeStorage;
    private EmployeeService _employeeService;

    public EquivalenceTests()
    {
        _clientStorage = new ClientStorage();
        _clientService = new ClientService(_clientStorage);
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
    [Fact]
    public void GetHashCodeNecessityPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryAccount(clientsBankList, currencies);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new Client(client.Name, client.Surname, client.NumPassport, client.Age, client.Phone,
            client.AccountNumber, client.Balance, client.DateBirthday);
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
        var newClient = new Client(client.Name, client.Surname, client.NumPassport, client.Age, client.Phone,
            client.AccountNumber, client.Balance, client.DateBirthday);
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
        var newEmployee = new Employee(employee.Name, employee.Surname, employee.NumPassport, employee.Age,
            employee.Phone,
            employee.Position, employee.StartDate, employee.Salary, employee.DateBirthday);
        //Act
        bool result = employeesBankList.Contains(newEmployee);
        //Assert
        Assert.Equal(result, employeesBankList.Contains(employeesBankList.First()));
    }

    [Fact]
    public void AddClientPositiveDictionaryTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        _clientService.AddClient(clientsBankDictionaryAccount);
        //Act
        var clients = _clientStorage.Get(new SearchRequest());
        //Assert
        Assert.Equal(clients, clientsBankList);
        //Assert.Throws<ClientNotFoundException>(() => clientService.AddClient(clientsBankDictionaryAccount));
    }
    [Fact]
    public void AddClientNegativeDictionaryTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        clientsBankList.First().Name = "";
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        //Act
        //_clientService.AddClient(clientsBankDictionaryAccount);     
        //Assert
        Assert.Throws<ArgumentException>(() => _clientService.AddClient(clientsBankDictionaryAccount));
        
    }

    [Fact]
    public void AddAccountPositiveTest()
    {
        //Arrange
        var account = testDataGenerator.GenerateAccountsArray(1, "EUR");
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        _clientService.AddClient(clientsBankDictionaryAccount);
        _clientService.AddAccount(clientsBankDictionaryAccount.Keys.First(), account);
        //Act
        var result = _clientService.GetAccount(clientsBankDictionaryAccount.Keys.First(),account[0].Currency.Code);
        //Assert
        Assert.Equal(account[0], result);
    }

    [Fact]
    public void UpdateAccountPositiveTest()
    {
        //Arrange
        var account = testDataGenerator.GenerateAccountsArray(1, "EUR");
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        _clientService.AddClient(clientsBankDictionaryAccount);
        _clientService.AddAccount(clientsBankDictionaryAccount.Keys.First(), account);
        _clientService.UpdateAccount(clientsBankDictionaryAccount.Keys.First(), 205m, "EUR", currencies);
        //Act
        var result = _clientService.GetAccount(clientsBankDictionaryAccount.Keys.First(),account[0].Currency.Code);
        //Assert
        Assert.Equal(205m, result.Ammount);
    }

    [Fact]
    public void FilterClientPositiveTest()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        _clientService.AddClient(clientsBankDictionaryAccount);
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
        _employeeService.AddEmployee(employeesBankList);
        //Act
        var employee = _employeeStorage.Get(new SearchRequest());
        //Assert
        Assert.Equal(employee, employeesBankList);
    }

    [Fact]
    public void FilterEmployeePositiveTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        _employeeService.AddEmployee(employeesBankList);
        var searchRequest = new SearchRequest { Name = employeesBankList[0].Name };
        //Act
        var filteredEmployees = _employeeService.FilterEmployees(searchRequest);
        //Assert
        Assert.Equal(employeesBankList[0].NumPassport, filteredEmployees.First().NumPassport);
    }
}