using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage : IClientStorage
{
    private Dictionary<Client, Account[]> Clients { get; set; }

    public ClientStorage()
    {
        Clients = new Dictionary<Client, Account[]>();
    }
    public Dictionary<Client, Account[]> GetAllClients()
    {
        return new Dictionary<Client, Account[]>(Clients);
    }

    public void AddClientToCollection(Dictionary<Client, Account[]> clients)
    {
        foreach (var client in clients)
        {
            Clients.Add(client.Key, client.Value);
        }
    }
    
    public void Add(Client client)
    {
        if (Clients.Any(c => c.Key.ClientId == client.ClientId))
        {
            throw new InvalidOperationException($"Клиент с ID {client.ClientId} уже добавлен.");
        }

        Clients.Add(client, new Account[] { });
    }

    public List<Client> Get(SearchRequest searchRequest)
    {
        IEnumerable<Client> request = Clients.Keys;
        if (!string.IsNullOrWhiteSpace(searchRequest.Name))
        {
            request = request.Where(c => c.Name == searchRequest.Name);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Surname))
        {
            request = request.Where(c => c.Surname == searchRequest.Surname);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Phone))
        {
            request = request.Where(c => c.Phone == searchRequest.Phone);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.NumPassport))
        {
            request = request.Where(c => c.NumPassport == searchRequest.NumPassport);
        }

        if (searchRequest.DateStart != null && searchRequest.DateEnd != null &&
            searchRequest.DateStart <= searchRequest.DateEnd)
        {
            request = request.Where(c => 
                c.DateBirthday >= searchRequest.DateStart && c.DateBirthday <= searchRequest.DateEnd); 
        }
        return request.ToList();
    }

    public void Update(Client client)
    {
        var oldClients = Clients.FirstOrDefault(c => c.Key.ClientId == client.ClientId).Key;
        
        if (oldClients != null)
        {
            oldClients.Name = client.Name;
            oldClients.Surname = client.Surname;
            oldClients.Phone = client.Phone;
            oldClients.NumPassport = client.NumPassport;
            oldClients.DateBirthday = client.DateBirthday;
            oldClients.AccountNumber = client.AccountNumber;
            oldClients.Balance = client.Balance;
        }
        else
        {
            throw new EntityNotFoundException("Клиент не найден с данным ID: " + client.ClientId);
        }
    }

    public void Delete(Client client)
    {
        if (Clients.ContainsKey(client))
        {
            Clients.Remove(client);        }
        else
        {
            throw new EntityNotFoundException("Клиент не найден.");
        }
    }

    public void AddAccount(Client client, Account[] accounts)
    {
        Clients[client] = Clients[client].Concat(accounts).ToArray();
    }
    
    public Account GetAccount(Client client, string currencyCode)
    {
        if (Clients.ContainsKey(client))
        {
            var account = Clients[client].FirstOrDefault(c=>c.Currency.Code == currencyCode);
            if (account!=null)
                return account;
            throw new EntityNotFoundException("У клиента нет счета с валютой " + currencyCode);
        }
        throw new EntityNotFoundException("Клиент не найден.");
    }

    public void UpdateAccount(Client client, decimal ammount, string currencyCode)
    {
        var account = Clients[client].FirstOrDefault(c=>c.Currency.Code == currencyCode);
        if (account!=null)
            account.Ammount = ammount;
        else
        {
            throw new EntityNotFoundException("У клиента нет счета с валютой " + currencyCode);
        }
    }

    public void DeleteAccount(Client client, Account account)
    {
        if (Clients.ContainsKey(client))
        {
            var accounts = Clients[client];
            
            if (accounts.Contains(account))
            {
                Clients[client] = accounts.Where(a => a != account).ToArray();
            }
            else
            {
                throw new EntityNotFoundException("Аккаунт не найден.");
            }
        }
        else
        {
            throw new EntityNotFoundException("Клиент не найден.");
        }
    }
    
    public Client? SearchYoungClient()
    {
        return Clients.MinBy(c => c.Key.Age).Key;
    }
    public Client? SearchOldClient()
    {
        return Clients.MaxBy(c => c.Key.Age).Key;
    }
    public int? SearchAverageAgeClient()
    {
        return (int)Clients.Average(c => c.Key.Age);
    }
}