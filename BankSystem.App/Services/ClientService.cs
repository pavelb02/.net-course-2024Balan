using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private readonly IClientStorage _clientStorage;

    public ClientService(IClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }

    public Client GetClient(Guid clientId)
    {
        return _clientStorage.GetById(clientId);
    }
    
    public void DeleteClient(Guid clientId)
    {
        _clientStorage.Delete(clientId);
    }
    
    public void DeleteAccount(Guid clientId, Guid accountId)
    {
        _clientStorage.DeleteAccount(clientId, accountId);
    }

    public void AddClients(List<Client> clientsList)
    {
        try
        {
            foreach (var client in clientsList.Where(client => ValidateAddClient(client)))
            {
                _clientStorage.Add(client);
            }
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении клиента: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Произошла непредвиденная ошибка при добавлении клиента.", ex);
        }
    }

    public void AddAccount(Guid clientId, Account account)
    {
        try
        {
            if (ValidateAccount(account))
                _clientStorage.AddAccount(clientId, account); 
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении аккаунта: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Произошла непредвиденная ошибка при добавлении аккаунта.", ex);
        }
    }

    public void UpdateClient(Guid clientId, Client newClient)
    {
        try
        {
            if (ValidateAddClient(newClient))
            {
                _clientStorage.Update(clientId, newClient);
            }
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при обновлении клиента: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Произошла непредвиденная ошибка при обновлении клиента.", ex);
        }
    }

    public List<Client> FilterClients(SearchRequest searchRequest)
    {
        var filteredClients = _clientStorage.GetCollection(searchRequest);
        return filteredClients;
    }

    private static bool ValidateCurrency(string currencyCode, Currency[] currencies)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentException("В метод передана пустая строка (или из пробелов) или null", nameof(currencyCode));
        }

        if (currencies.All(c => c.Code != currencyCode))
        {
            throw new EntityNotFoundException(nameof(currencyCode));
        }
        
        return true;
    }

    private static bool ValidateAccount(Account account)
    {
        if (account.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(account.Amount),
                "Сумма на счету не может быть меньше нуля.");
        }

        return true;
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