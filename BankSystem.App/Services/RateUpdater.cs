using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class RateUpdater
{
    private IClientStorage _clientStorage;

    public RateUpdater(IClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }

    public async Task ChargeInterestAsync(decimal interest, CancellationToken token)
    {
        var pageSize = 5;
        while (!token.IsCancellationRequested)
        {
            var pageNumber = 1;
            var flag = true;
            while (flag)
            {
                var clients = await _clientStorage.GetCollectionAsync(new SearchRequest
                    { PageSize = pageSize, PageNumber = pageNumber });
                if (clients.Count < pageSize)
                    flag = false;
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Операция прервана");
                    return;
                }

                foreach (var client in clients)
                {
                    foreach (var account in client.AccountsClient)
                    {
                        account.Amount += account.Amount * (decimal)0.1;
                    }

                    await _clientStorage.UpdateAsync(client.Id, client);
                }

                pageNumber++;
            }

            await Task.Delay(5000, token);
        }
    }
}