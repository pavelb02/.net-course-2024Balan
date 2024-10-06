using BankSystem.App.Exeptions;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    TestDataGenerator testDataGenerator = new TestDataGenerator();
    private static ClientStorage clientStorage = new();
    private static EmployeeStorage employeeStorage = new();
    private ClientService clientService = new ClientService(clientStorage);
    private EmployeeService employeeService = new(employeeStorage);

    string[] positions =
    {
        "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor",
        "IT specialist"
    };

    [Fact]
    public void GetHashCodeNecessityPositiveTest()
    {
        //Arrange
        Currency[] currencies =
        {
            new("USD", "Dollar USA", "$", 16.3m),
            new("EUR", "Euro", "€", 18.6m),
            new("RUP", "Russian ruble", "₽", 0.185m)
        };
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
        Currency[] currencies =
        {
            new("USD", "Dollar USA", "$", 16.3m),
            new("EUR", "Euro", "€", 18.6m),
            new("RUP", "Russian ruble", "₽", 0.185m)
        };
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
        clientService.AddClient(clientsBankDictionaryAccount);
        //Act
        var clients = clientStorage.GetAllClients();
        //Assert
        Assert.Equal(clients, clientsBankDictionaryAccount);
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
        clientService.AddClient(clientsBankDictionaryAccount);     
        //Assert
        Assert.Throws<ArgumentException>(() => clientService.AddClient(clientsBankDictionaryAccount));
    }

    [Fact]
    public void AddAccountPositiveTest()
    {
        //Arrange
        var account = testDataGenerator.GenerateAccountsArray(1, "EUR");
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        clientService.AddClient(clientsBankDictionaryAccount);
        clientService.AddAccount(clientsBankDictionaryAccount.Keys.First(), account);
        var clients = clientStorage.GetAllClients();
        //Act
        var result = clients[clients.Keys.First()];
        //Assert
        Assert.Contains(account[0], result);
    }

    [Fact]
    public void UpdateAccountPositiveTest()
    {
        //Arrange
        var account = testDataGenerator.GenerateAccountsArray(1, "EUR");
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount =
            testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList);
        clientService.AddClient(clientsBankDictionaryAccount);
        clientService.AddAccount(clientsBankDictionaryAccount.Keys.First(), account);
        clientService.UpdateAccount(clientsBankDictionaryAccount.Keys.First(), 205m, "EUR");
        var clients = clientStorage.GetAllClients();
        //Act
        var result = clients[clients.Keys.First()].First(a => a.Currency.Code == "EUR");
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
        clientService.AddClient(clientsBankDictionaryAccount);
        var searchRequest = new SearchRequest { NumPassport = clientsBankList[0].NumPassport };
        //Act
        clientService.FilterClients(searchRequest);
        //Assert
        var filteredClients = clientStorage.GetAllClients();
        Assert.Single(filteredClients);
        Assert.Equal(clientsBankList[0].NumPassport, filteredClients.Keys.First().NumPassport);
    }

    [Fact]
    public void AddEmployeePositiveListTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        employeeService.AddEmployee(employeesBankList);
        //Act
        var employee = employeeStorage.GetAllEmployees();
        //Assert
        Assert.Equal(employee, employeesBankList);
    }
    [Fact]
    public void AddEmployeeNegativeListTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        employeesBankList.First().NumPassport = "";
        //Act
        employeeService.AddEmployee(employeesBankList);     
        //Assert
        Assert.Throws<ArgumentException>(() => employeeService.AddEmployee(employeesBankList));
    }

    [Fact]
    public void FilterEmployeePositiveTest()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        employeeService.AddEmployee(employeesBankList);
        var searchRequest = new SearchRequest { Name = employeesBankList[0].Name };
        //Act
        employeeService.FilterEmployees(searchRequest);
        //Assert
        var filteredEmployees = employeeStorage.GetAllEmployees();
        Assert.Equal(employeesBankList[0].NumPassport, filteredEmployees.First().NumPassport);
    }
}