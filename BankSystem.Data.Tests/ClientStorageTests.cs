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
    CancellationToken cancellationToken = CancellationToken.None;

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
        await _clientStorage.AddAsync(client, cancellationToken);
        //Assert
        var addedClient = _clientStorage.GetByIdAsync(clientsBankList.First().Id, cancellationToken);
        Assert.NotNull(addedClient);
    }
    [Fact]
    public async Task SearchYoungClientAsyncPositiveTest()
    {
        //Arrange
        var clientsBankList = _testDataGenerator.GenerateClientsBankList(10);
        foreach (var client in clientsBankList)
        {
            await _clientStorage.AddAsync(_mapper.Map<Client>(client), cancellationToken);
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
            _clientStorage.AddAsync(_mapper.Map<Client>(client), cancellationToken);
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
            await _clientStorage.AddAsync(_mapper.Map<Client>(client), cancellationToken);
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