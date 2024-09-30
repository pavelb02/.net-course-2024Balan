using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    TestDataGenerator testDataGenerator = new TestDataGenerator();
    [Fact]
    public void GetHashCodeNecessityPositivTest()
    {
        //Arrange
        Currency[] currencies =
        {
            new("USD", "Dollar USA", "$", 16.3m),
            new("EUR", "Euro", "€", 18.6m),
            new("RUP", "Russian ruble", "₽", 0.185m)
        };
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryAccount(clientsBankList, currencies);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new Client(client.Name, client.Surname, client.NumPassport, client.Age, client.Phone,
            client.AccountNumber, client.Balance);
        //Act
        Account result = clientsBankDictionaryAccount[newClient];
        //Assert
        Assert.Equal(result, clientsBankDictionaryAccount[client]);
    }
    [Fact]
    public void GetHashCodeNecessityPositivMultiAccountTest()
    {
        //Arrange
        Currency[] currencies =
        {
            new("USD", "Dollar USA", "$", 16.3m),
            new("EUR", "Euro", "€", 18.6m),
            new("RUP", "Russian ruble", "₽", 0.185m)
        };
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList, currencies);
        var client = clientsBankDictionaryAccount.Keys.First();
        var newClient = new Client(client.Name, client.Surname, client.NumPassport, client.Age, client.Phone,
            client.AccountNumber, client.Balance);
        //Act
        Account[] result = clientsBankDictionaryAccount[newClient];
        //Assert
        Assert.Equal(result, clientsBankDictionaryAccount[clientsBankDictionaryAccount.Keys.First()]);
    }
    [Fact]
    public void GetHashCodeNecessityPositivListTest()
    {
        //Arrange
        string[] positions = { "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor", "IT specialist" };
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        var employee = employeesBankList.First();
        var newEmployee = new Employee(employee.Name, employee.Surname, employee.NumPassport, employee.Age, employee.Phone,
            employee.Position, employee.StartDate, employee.Salary);
        //Act
        bool result = employeesBankList.Contains(newEmployee);
        //Assert
        Assert.Equal(result, employeesBankList.Contains(employeesBankList.First()));
    }
}