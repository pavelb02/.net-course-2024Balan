using AutoMapper;
using BankSystem.App.Mapping;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    TestDataGenerator _testDataGenerator = new();
    private ClientStorage _clientStorage;
   
    private IMapper _mapper;

    public ClientStorageTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ClientProfile>(); 
        });
    
        _mapper = mapperConfig.CreateMapper();
        _clientStorage = new ClientStorage();
    }
    [Fact]
    public async Task AddClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(1);
        var client = _mapper.Map<Client>(clientsBankList.First());
        //Act
        await _clientStorage.AddAsync(client);
        //Assert
        var addedClient = _clientStorage.GetByIdAsync(clientsBankList.First().Id);
        Assert.NotNull(addedClient);
    }
    [Fact]
    public async Task SearchYoungClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            await _clientStorage.AddAsync(_mapper.Map<Client>(client));
        }
        
        var youngClient = clientsBankList.MinBy(c => c.DateBirthday);
        //Act
        var youngClientMethod = await _clientStorage.SearchYoungClientAsync();
        //Assert
        Assert.Equal(youngClient.DateBirthday, youngClientMethod.DateBirthday);
    }
    [Fact]
    public async Task SearchOldClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            _clientStorage.AddAsync(_mapper.Map<Client>(client));
        }
        
        var oldClient = clientsBankList.MaxBy(c => c.DateBirthday);
        //Act
        var oldClientMethod = await _clientStorage.SearchOldClientAsync();
        //Assert
        Assert.Equal(oldClient.DateBirthday, oldClientMethod.DateBirthday);
    }
    [Fact]
    public async Task SearchAverageClientPositiv()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            await _clientStorage.AddAsync(_mapper.Map<Client>(client));
        }
        var dateNow = DateTime.Now;
        var averageAge = (int)clientsBankList.Average(c => dateNow.Year - c.DateBirthday.Year -
                                                           (dateNow.DayOfYear < c.DateBirthday.DayOfYear ? 1 : 0));
        //Act
        var averageAgeClientMethod = await _clientStorage.SearchAverageAgeClientAsync();
        //Assert
        Assert.Equal(averageAge, averageAgeClientMethod);
    }
}

public class EmployeeStorageTests
{
    EmployeeStorage _employeeStorage = new EmployeeStorage();
    TestDataGenerator _testDataGenerator = new TestDataGenerator();
    string[] positions = { "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor", "IT specialist" };

    [Fact]
    public async Task AddEmployeeToCollectionAsyncPositiveTest()
    {
        //Arrange
        var employeesBankList = _testDataGenerator.GenerateEmployeesBankList(1, positions);
        //Act
        await _employeeStorage.AddAsync(employeesBankList.First());
        //Assert
        Assert.NotNull(_employeeStorage.GetCollectionAsync(new SearchRequest()));
    }
    [Fact]
    public async Task SearchYoungEmployeePositiv()
    {
        //Arrange
        var employeesBankList = _testDataGenerator.GenerateEmployeesBankList(10, positions);
        foreach (var employee in employeesBankList)
        {
            await _employeeStorage.AddAsync(employee);
        }
        var youngEmployee = employeesBankList.MinBy(e => e.DateBirthday);
        //Act
        var youngEmployeeMethod = await _employeeStorage.SearchYoungEmployee();
        //Assert
        Assert.Equal(youngEmployee.DateBirthday, youngEmployeeMethod.DateBirthday);
    }
    [Fact]
    public async Task SearchOldEmployeePositiv()
    {
        //Arrange
        var employeesBankList = _testDataGenerator.GenerateEmployeesBankList(10, positions);
        foreach (var employee in employeesBankList)
        {
            await _employeeStorage.AddAsync(employee);
        }
        var oldEmployee = employeesBankList.MaxBy(e => e.DateBirthday);
        //Act
        var oldEmployeeMethod = await _employeeStorage.SearchOldEmployee();
        //Assert
        Assert.Equal(oldEmployee.DateBirthday, oldEmployeeMethod.DateBirthday);
    }
    [Fact]
    public async Task SearchAverageEmployeePositiv()
    {
        //Arrange
        var employeesBankList = _testDataGenerator.GenerateEmployeesBankList(10, positions);
        foreach (var employee in employeesBankList)
        {
            await _employeeStorage.AddAsync(employee);
        }
        var dateNow = DateTime.Now;
        var averageAge = (int)employeesBankList.Average(e => dateNow.Year - e.DateBirthday.Year -
                                                        (dateNow.DayOfYear < e.DateBirthday.DayOfYear ? 1 : 0));
        //Act
        var averageAgeEmployeeMethod = await _employeeStorage.SearchAverageAgeEmployee();
        //Assert
        Assert.Equal(averageAge, averageAgeEmployeeMethod);
    }
}