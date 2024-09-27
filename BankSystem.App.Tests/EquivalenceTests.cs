using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    [Fact]
    public void GetHashCodeNecessityPositivTest()
    {
        //Arrange
        var testDataGenerator = new TestDataGenerator();
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
        Assert.Equal(result, clientsBankDictionaryAccount[clientsBankDictionaryAccount.Keys.First()]);
    }

}