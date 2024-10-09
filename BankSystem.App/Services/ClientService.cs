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

    public void AddClient(Dictionary<Client, Account[]> clientsBankDictionaryAccount)
    {
        try
        {
            foreach (var client in clientsBankDictionaryAccount)
            {

                if (ValidateAddClient(client.Key))
                {
                    _clientStorage.Add(client.Key);
                    if (ValidateAccount(client.Value))
                    {
                        _clientStorage.AddAccounts(client.Key, client.Value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Ошибка при добавлении клиента: {ex.Message}", ex);
        }
    }

    public void AddAccounts(Client client, Account[] accounts)
    {
        var clientOne = _clientStorage.Get(new SearchRequest { NumPassport = client.NumPassport}).First();
        try
        {
            if (clientOne == null)
                throw new EntityNotFoundException(nameof(client));
            if (ValidateAccount(accounts))
                _clientStorage.AddAccounts(clientOne, accounts); 
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Ошибка при добавлении аккаунта: {ex.Message}");
        }
    }

    public Account GetAccount(Client client, string currencyCode)
    {
        return _clientStorage.GetAccount(client, currencyCode);
    }
    
    public void UpdateAccount(Client client, decimal ammount, string currencyCode, Currency[] currencies)
    {
        var clientOne = _clientStorage.Get(new SearchRequest { NumPassport = client.NumPassport }).First();
        try
        {
            if (clientOne == null)
                throw new EntityNotFoundException(nameof(client));
            if (ValidateCurrency(currencyCode, currencies))
            {
                if (ammount < 0)
                    throw new ArgumentOutOfRangeException(nameof(ammount),
                        "Сумма на счету не может быть меньше нуля.");
                _clientStorage.UpdateAccount(clientOne, ammount, currencyCode);
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Ошибка при обновлении аккаунта: {ex.Message}");
        }
    }

    public List<Client> FilterClients(SearchRequest searchRequest)
    {
        var filteredClients = _clientStorage.Get(searchRequest);
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
    private static bool ValidateAccount(Account[] accountsArray)
    {
        foreach (var account in accountsArray)
        {
            if (account.Ammount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(account.Ammount),
                    "Сумма на счету не может быть меньше нуля.");
            }
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