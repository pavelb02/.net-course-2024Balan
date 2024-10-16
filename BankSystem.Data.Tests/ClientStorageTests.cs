using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    ClientStorage _clientStorage = new ClientStorage();
    TestDataGenerator testDataGenerator = new TestDataGenerator();
    [Fact]
    public void AddClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(1);
        //Act
        _clientStorage.Add(clientsBankList.First());
        //Assert
        var addedClient = _clientStorage.GetById(clientsBankList.First().Id);
        Assert.NotNull(addedClient);
    }
    [Fact]
    public void SearchYoungClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            _clientStorage.Add(client);
        }
        
        var youngClient = clientsBankList.MinBy(c => c.Age);
        //Act
        var youngClientMethod = _clientStorage.SearchYoungClient();
        //Assert
        Assert.Equal(youngClient.Age, youngClientMethod.Age);
    }
    [Fact]
    public void SearchOldClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            _clientStorage.Add(client);
        }
        
        var oldClient = clientsBankList.MaxBy(c => c.Age);
        //Act
        var oldClientMethod = _clientStorage.SearchOldClient();
        //Assert
        Assert.Equal(oldClient.Age, oldClientMethod.Age);
    }
    [Fact]
    public void SearchAverageClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            _clientStorage.Add(client);
        }
        
        var averageAgeClient = (int)clientsBankList.Average(c => c.Age);
        //Act
        var averageAgeClientMethod = _clientStorage.SearchAverageAgeClient();
        //Assert
        Assert.Equal(averageAgeClient, averageAgeClientMethod);
    }
}

public class EmployeeStorageTests
{
    EmployeeStorage _employeeStorage = new EmployeeStorage();
    TestDataGenerator testDataGenerator = new TestDataGenerator();
    string[] positions = { "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor", "IT specialist" };

    [Fact]
    public void AddEmployeeToCollectionPositiv()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(1, positions);
        //Act
        _employeeStorage.Add(employeesBankList.First());
        //Assert
        Assert.NotNull(_employeeStorage.GetCollection(new SearchRequest()));
        Assert.True(_employeeStorage.GetCollection(new SearchRequest()).SequenceEqual(employeesBankList));
    }
    [Fact]
    public void SearchYoungEmployeePositiv()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        foreach (var employee in employeesBankList)
        {
            _employeeStorage.Add(employee);
        }
        var youngEmployee = employeesBankList.MinBy(c => c.Age);
        //Act
        var youngEmployeeMethod = _employeeStorage.SearchYoungEmployee();
        //Assert
        Assert.Equal(youngEmployee.Age, youngEmployeeMethod.Age);
    }
    [Fact]
    public void SearchOldEmployeePositiv()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        foreach (var employee in employeesBankList)
        {
            _employeeStorage.Add(employee);
        }
        var oldEmployee = employeesBankList.MaxBy(c => c.Age);
        //Act
        var oldEmployeeMethod = _employeeStorage.SearchOldEmployee();
        //Assert
        Assert.Equal(oldEmployee.Age, oldEmployeeMethod.Age);
    }
    [Fact]
    public void SearchAverageEmployeePositiv()
    {
        //Arrange
        var employeesBankList = testDataGenerator.GenerateEmployeesBankList(10, positions);
        foreach (var employee in employeesBankList)
        {
            _employeeStorage.Add(employee);
        }
        var averageAgeEmployee = (int)employeesBankList.Average(c => c.Age);
        //Act
        var averageAgeEmployeeMethod = _employeeStorage.SearchAverageAgeEmployee();
        //Assert
        Assert.Equal(averageAgeEmployee, averageAgeEmployeeMethod);
    }
}