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
            
            while (true)
            {
                var clients = await _clientStorage.GetCollectionAsync(new SearchRequest
                    { PageSize = pageSize, PageNumber = pageNumber });
                
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Операция прервана");
                    return;
                }

                foreach (var client in clients)
                {
                    foreach (var account in client.AccountsClient)
                    {
                        account.Amount += account.Amount * interest;
                    }

                    await _clientStorage.UpdateAsync(client);
                }

                pageNumber++;
                
                if (clients.Count < pageSize)
                    break;
            }

            var dateNow = DateTime.Now;
            var nextMonth = new DateTime(dateNow.Year, dateNow.Month, 1).AddMonths(1);
            var delay = nextMonth - dateNow;

            await Task.Delay(delay, token);
        }
    }
}