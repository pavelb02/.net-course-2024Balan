using BankSystem.App.Services;
using BankSystem.Data.Storages;

namespace BankSystem.Data.Tests;

public class RateUpdaterTests
{
    private ClientStorage _clientStorage;
    private RateUpdater rateUdapter;

    public RateUpdaterTests()
    {
        _clientStorage = new ClientStorage();
        rateUdapter = new RateUpdater(_clientStorage);
    }

    [Fact]
    public async Task ChargeInterestAsyncPositiveTest()
    {
        //Arrange
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;
        decimal interest = (decimal)0.1;

        //Act
        var updateTask = rateUdapter.ChargeInterestAsync(interest, token);
        await Task.Delay(15100);
        cancelTokenSource.Cancel();

        //Assert
        try
        {
            await updateTask;
            Assert.True(false, "Ожидалось исключение TaskCanceledException, но оно не было выброшено.");
        }
        catch (TaskCanceledException)
        {
            Assert.True(true, "Операция была успешно отменена.");
        }
    }
}