using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private readonly IClientStorage _clientStorage;
    private readonly ICurrencyService _currencyService;

    public ClientService(IClientStorage clientStorage, ICurrencyService currencyService)
    {
        _clientStorage = clientStorage;
        _currencyService = currencyService;
    }

    public async Task<Client> GetClientAsync(Guid clientId)
    {
        return await _clientStorage.GetByIdAsync(clientId);
    }
    
    public async Task DeleteClientAsync(Guid clientId)
    {
        await _clientStorage.DeleteAsync(clientId);
    }
    
    public async Task DeleteAccountAsync(Guid accountId)
    {
        await _clientStorage.DeleteAccountAsync(accountId);
    }

    public async Task AddClientAsync(Client client, string currencyCode)
    {
        try
        {
            if (! await ValidateAddClientAsync(client)) return;
            var currencyId = await _currencyService.GetCurrencyAsync(currencyCode);
            var account = new Account(client.Id, currencyId);
            client.AccountsClient.Add(account);
            await _clientStorage.AddAsync(client);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении клиента: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task AddAccountAsync(Guid clientId, string currencyCode)
    {
        try
        {
            var client = await _clientStorage.GetByIdAsync(clientId);
            var currencyId = await _currencyService.GetCurrencyAsync(currencyCode);
            var account = new Account(client.Id, currencyId);
            await _clientStorage.AddAccountAsync(clientId, account);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении аккаунта: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task UpdateClientAsync(Guid clientId, Client newClient)
    {
        try
        {
            if (await ValidateAddClientAsync(newClient))
            {
                var client = await _clientStorage.GetByIdAsync(clientId);
                var accounts = client.AccountsClient;
                var currencyId = await _currencyService.GetCurrencyAsync("EUR");
                accounts.Add(new Account(clientId, currencyId));
                foreach (var account in accounts)
                {
                    newClient.AccountsClient.Add(account);
                }
                await _clientStorage.UpdateAsync(clientId, newClient);
            }
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при обновлении клиента: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<List<Client>> FilterClientsAsync(SearchRequest searchRequest)
    {
        var filteredClients = await _clientStorage.GetCollectionAsync(searchRequest);
        return filteredClients;
    }

    public async Task<bool> Debit(WithdrawalRequest withdrawalRequest)
    {
        var client = await _clientStorage.GetByIdAsync(withdrawalRequest.ClientId);
        var currencyId = await _currencyService.GetCurrencyAsync(withdrawalRequest.CurrencyCode);
        var account = client.AccountsClient.FirstOrDefault(a => a.CurrencyId == currencyId);
        if (account == null)
        {
            Console.WriteLine($"Аккаунт клиента с валютой {withdrawalRequest.CurrencyCode} не найден.");
            return false;
        }

        if (account.Amount < withdrawalRequest.WithdrawalAmount)
        {
            Console.WriteLine("Недостаточно средств на счете.");
            return false;
        }
        account.Amount -= withdrawalRequest.WithdrawalAmount;
        
        await _clientStorage.UpdateAsync(client.Id, client);
        return true;
    }

    private static Task<bool> ValidateAddClientAsync(Client client)
    {
        if (string.IsNullOrWhiteSpace(client.Name))
        {
            throw new ArgumentException("Имя не может быть null, пустым или состоять только из пробелов.", nameof(client.Name));
        }

        if (string.IsNullOrWhiteSpace(client.Name))
        {
            throw new ArgumentException("Фамиилия не может быть null, пустой или состоять только из пробелов.", nameof(client.Surname));
        }

        if (string.IsNullOrWhiteSpace(client.NumPassport))
        {
            throw new ArgumentException("Номер паспорта не может быть null, пустым или состоять только из пробелов.", nameof(client.NumPassport));
        }
        var dateNow = DateTime.Now;
        int age = dateNow.Year - client.DateBirthday.Year;
        if (dateNow.DayOfYear < client.DateBirthday.DayOfYear)
        {
            age--;
        }
        if (age < 18)
        {
            throw new PersonAgeException(age);
        }

        if (age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст должен быть положительным.");
        }

        return Task.FromResult(true);
    }
}