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

    public Client GetClient(Guid clientId)
    {
        return _clientStorage.GetById(clientId);
    }
    
    public void DeleteClient(Guid clientId)
    {
        _clientStorage.Delete(clientId);
    }
    
    public void DeleteAccount(Guid accountId)
    {
        _clientStorage.DeleteAccount(accountId);
    }

    public void AddClients(Client client, string currencyCode)
    {
        try
        {
            if (!ValidateAddClient(client)) return;
            var currencyId = _currencyService.GetGurrency(currencyCode);
            var account = new Account(client.Id, currencyId);
            client.AccountsClient.Add(account);
            _clientStorage.Add(client);
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

    public void AddAccount(Guid clientId, string currencyCode)
    {
        try
        {
            var client = _clientStorage.GetById(clientId);
            var currencyId = _currencyService.GetGurrency(currencyCode);
            var account = new Account(client.Id, currencyId);
            _clientStorage.AddAccount(clientId, account);
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

    public void UpdateClient(Guid clientId, Client newClient)
    {
        try
        {
            if (ValidateAddClient(newClient))
            {
                var accounts = _clientStorage.GetById(clientId).AccountsClient;
                var currencyId = _currencyService.GetGurrency("EUR");
                accounts.Add(new Account(clientId, currencyId));
                foreach (var account in accounts)
                {
                    newClient.AccountsClient.Add(account);
                }
                _clientStorage.Update(clientId, newClient);
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

    public List<Client> FilterClients(SearchRequest searchRequest)
    {
        var filteredClients = _clientStorage.GetCollection(searchRequest);
        return filteredClients;
    }

    private static bool ValidateAddClient(Client client)
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

        if (client.Age < 18)
        {
            throw new PersonAgeException(client.Age);
        }

        if (client.Age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(client.Age), "Возраст должен быть положительным.");
        }

        return true;
    }
}