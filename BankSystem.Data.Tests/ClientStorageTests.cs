using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    ClientStorage clientStorage = new ClientStorage();
    TestDataGenerator testDataGenerator = new TestDataGenerator();
    Currency[] currencies =
    {
        new("USD", "Dollar USA", "$", 16.3m),
        new("EUR", "Euro", "€", 18.6m),
        new("RUP", "Russian ruble", "₽", 0.185m)
    };
    [Fact]
    public void AddClientToCollectionPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList, currencies);
        //Act
        clientStorage.AddClientToCollection(clientsBankDictionaryAccount);
        //Assert
        Assert.NotNull(clientStorage.GetAllClients());
        Assert.True(clientStorage.GetAllClients().SequenceEqual(clientsBankDictionaryAccount));
    }
    [Fact]
    public void SearchYoungClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList, currencies);
        clientStorage.AddClientToCollection(clientsBankDictionaryAccount);
        
        var youngClient = clientsBankDictionaryAccount.MinBy(c => c.Key.Age).Key;
        //Act
        var youngClientMethod = clientStorage.SearchYoungClient();
        //Assert
        Assert.Equal(youngClient.Age, youngClientMethod.Age);
    }
    [Fact]
    public void SearchOldClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList, currencies);
        clientStorage.AddClientToCollection(clientsBankDictionaryAccount);
        
        var oldClient = clientsBankDictionaryAccount.MaxBy(c => c.Key.Age).Key;
        //Act
        var oldClientMethod = clientStorage.SearchOldClient();
        //Assert
        Assert.Equal(oldClient.Age, oldClientMethod.Age);
    }
    [Fact]
    public void SearchAverageClientPositiv()
    {
        //Arrange
        var clientsBankList = testDataGenerator.GenerateClientsBankList(10);
        var clientsBankDictionaryAccount = testDataGenerator.GenerateClientsBankDictionaryMultiAccount(clientsBankList, currencies);
        clientStorage.AddClientToCollection(clientsBankDictionaryAccount);
        
        var averageAgeClient = (int)clientsBankDictionaryAccount.Average(c => c.Key.Age);
        //Act
        var averageAgeClientMethod = clientStorage.SearchAverageAgeClient();
        //Assert
        Assert.Equal(averageAgeClient, averageAgeClientMethod);
    }
}