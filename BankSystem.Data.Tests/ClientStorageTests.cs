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
        
        var youngClient = clientsBankList.MinBy(c => c.DateBirthday);
        //Act
        var youngClientMethod = _clientStorage.SearchYoungClient();
        //Assert
        Assert.Equal(youngClient.DateBirthday, youngClientMethod.DateBirthday);
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
        
        var oldClient = clientsBankList.MaxBy(c => c.DateBirthday);
        //Act
        var oldClientMethod = _clientStorage.SearchOldClient();
        //Assert
        Assert.Equal(oldClient.DateBirthday, oldClientMethod.DateBirthday);
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
        var dateNow = DateTime.Now;
        var averageAge = (int)clientsBankList.Average(c => dateNow.Year - c.DateBirthday.Year -
                                                           (dateNow.DayOfYear < c.DateBirthday.DayOfYear ? 1 : 0));
        //Act
        var averageAgeClientMethod = _clientStorage.SearchAverageAgeClient();
        //Assert
        Assert.Equal(averageAge, averageAgeClientMethod);
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
        var youngEmployee = employeesBankList.MinBy(e => e.DateBirthday);
        //Act
        var youngEmployeeMethod = _employeeStorage.SearchYoungEmployee();
        //Assert
        Assert.Equal(youngEmployee.DateBirthday, youngEmployeeMethod.DateBirthday);
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
        var oldEmployee = employeesBankList.MaxBy(e => e.DateBirthday);
        //Act
        var oldEmployeeMethod = _employeeStorage.SearchOldEmployee();
        //Assert
        Assert.Equal(oldEmployee.DateBirthday, oldEmployeeMethod.DateBirthday);
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
        var dateNow = DateTime.Now;
        var averageAge = (int)employeesBankList.Average(e => dateNow.Year - e.DateBirthday.Year -
                                                        (dateNow.DayOfYear < e.DateBirthday.DayOfYear ? 1 : 0));
        //Act
        var averageAgeEmployeeMethod = _employeeStorage.SearchAverageAgeEmployee();
        //Assert
        Assert.Equal(averageAge, averageAgeEmployeeMethod);
    }
}