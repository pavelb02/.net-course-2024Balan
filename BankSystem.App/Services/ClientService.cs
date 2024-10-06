using BankSystem.App.Exeptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
namespace BankSystem.App.Services;

public class ClientService
{
    ClientStorage _clientStorage;

    public ClientService(ClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }

    public void AddClient(Dictionary<Client, Account[]> clientsBankDictionaryAccount)
    {
        var clientsBankDictionaryAccountCorrect = new Dictionary<Client, Account[]>();
        foreach (var client in clientsBankDictionaryAccount)
        {
            try
            {
                if (ValidateAddClient(client))
                    clientsBankDictionaryAccountCorrect.Add(client.Key, client.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении клиента: {ex.Message}");
            }
        }

        _clientStorage.AddClientToCollection(clientsBankDictionaryAccountCorrect);
    }

    public void AddAccount(Client client, Account[] accounts)
    {
        var clientOne = _clientStorage.GetClient(client);
        try
        {
            if (clientOne == null)
                throw new EntityNotFoundException(nameof(client));
            if (ValidateAccount(accounts))
            {
                var updatedAccounts = clientOne.Value.Value.Concat(accounts).ToArray();
                _clientStorage.UpdateClientAccounts(client, updatedAccounts);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении аккаунта: {ex.Message}");
        }
    }
    
    public void UpdateAccount(Client client, decimal ammount, string currencyCode, Currency[] currencies)
    {
        var clientOne = _clientStorage.GetClient(client);
        try
        {
            if (clientOne == null)
                throw new EntityNotFoundException(nameof(client));
            if (ValidateCurrency(currencyCode, currencies))
            {
                var account = clientOne.Value.Value.First(a => a.Currency.Code == currencyCode);
                if (ammount < 0)
                    throw new ArgumentOutOfRangeException(nameof(ammount),
                        "Сумма на счету не может быть меньше нуля.");
                account.Ammount = ammount;
                _clientStorage.UpdateClientAccounts(client, clientOne.Value.Value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении аккаунта: {ex.Message}");
        }
    }

    public Dictionary<Client, Account[]> FilterClients(SearchRequest searchRequest)
    {
        var clients = _clientStorage.GetAllClients();
        IEnumerable<KeyValuePair<Client,Account[]>> request = clients;
        if (!string.IsNullOrWhiteSpace(searchRequest.Name))
        {
            request = request.Where(c => c.Key.Name == searchRequest.Name);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Surname))
        {
            request = request.Where(c => c.Key.Surname == searchRequest.Surname);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Phone))
        {
            request = request.Where(c => c.Key.Phone == searchRequest.Phone);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.NumPassport))
        {
            request = request.Where(c => c.Key.NumPassport == searchRequest.NumPassport);
        }

        if (searchRequest.DateStart != null && searchRequest.DateEnd != null &&
            searchRequest.DateStart <= searchRequest.DateEnd)
        {
            request = request.Where(c => 
                c.Key.DateBirthday >= searchRequest.DateStart && c.Key.DateBirthday <= searchRequest.DateEnd); 
        }
        return request.ToDictionary();
    }
    public bool ValidateCurrency(string currencyCode, Currency[] currencies)
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
    private bool ValidateAccount(Account[] accountsArray)
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

    private bool ValidateAddClient(KeyValuePair<Client, Account[]> client)
    {
        if (string.IsNullOrWhiteSpace(client.Key.Name))
        {
            throw new ArgumentException("Имя не может быть null, пустым или состоять только из пробелов.", nameof(client.Key.Name));
        }

        if (string.IsNullOrWhiteSpace(client.Key.Name))
        {
            throw new ArgumentException("Фамиилия не может быть null, пустой или состоять только из пробелов.", nameof(client.Key.Surname));
        }

        if (string.IsNullOrWhiteSpace(client.Key.NumPassport))
        {
            throw new ArgumentException("Номер паспорта не может быть null, пустым или состоять только из пробелов.", nameof(client.Key.NumPassport));
        }

        if (client.Key.Age < 18)
        {
            throw new PersonAgeException(client.Key.Age);
        }

        if (client.Key.Age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(client.Key.Age), "Возраст должен быть положительным.");
        }

        return true;
    }
}