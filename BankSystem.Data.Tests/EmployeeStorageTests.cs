using AutoMapper;
using BankSystem.App.Mapping;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests;


public class EmployeeStorageTests
{
    TestDataGenerator _testDataGenerator = new();
    private EmployeeStorage _employeeStorage;
   
    private IMapper _mapper;
    CancellationToken cancellationToken = CancellationToken.None;

    public EmployeeStorageTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EmployeeProfile>(); 
        });
    
        _mapper = mapperConfig.CreateMapper();
        _employeeStorage = new EmployeeStorage();
    }
    string[] positions = { "Cashier", "Service Specialist", "Counselor", "Manager", "Bank Accountant", "Financial Analyst", "Auditor", "IT specialist" };

    [Fact]
    public async Task AddEmployeeToCollectionAsyncPositiveTest()
    {
        //Arrange
        var employeesBankListDto = _testDataGenerator.GenerateEmployeesBankList(1, positions);
        var employee = _mapper.Map<Employee>(employeesBankListDto.First());
        //Act
        await _employeeStorage.AddAsync(employee, cancellationToken);
        //Assert
        Assert.NotNull(_employeeStorage.GetCollectionAsync(new SearchRequest(), cancellationToken));
    }
    [Fact]
    public async Task SearchYoungEmployeePositiv()
    {
        //Arrange
        var employeesBankListDto = _testDataGenerator.GenerateEmployeesBankList(3, positions);
        var employeesBankList = _mapper.Map<List<Employee>>(employeesBankListDto);
        foreach (var employee in employeesBankList)
        {
            await _employeeStorage.AddAsync(employee, cancellationToken);
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
        var employeesBankListDto = _testDataGenerator.GenerateEmployeesBankList(3, positions);
        var employeesBankList = _mapper.Map<List<Employee>>(employeesBankListDto);
        foreach (var employee in employeesBankList)
        {
            await _employeeStorage.AddAsync(employee, cancellationToken);
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
        var employeesBankListDto = _testDataGenerator.GenerateEmployeesBankList(3, positions);
        var employeesBankList = _mapper.Map<List<Employee>>(employeesBankListDto);
        foreach (var employee in employeesBankList)
        {
            await _employeeStorage.AddAsync(employee, cancellationToken);
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