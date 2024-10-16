using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage : IClientStorage
{
    private BankSystemDbContext _dbContext;

    public ClientStorage()
    {
        _dbContext = new BankSystemDbContext();
    }

    public void Add(Client client)
    {
        if (_dbContext.Clients.Any(c => c.ClientId == client.ClientId))
        {
            throw new InvalidOperationException($"Клиент с ID {client.ClientId} уже существует.");
        }
        _dbContext.Clients.Add(client);
        _dbContext.SaveChanges();
    }

    public Client GetById(Guid clientId)
    {
        return _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
    }

    public List<Client> GetCollection(SearchRequest searchRequest)
    {
        IEnumerable<Client> request = _dbContext.Clients;
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

    public void Update(Guid clientId, Client client)
    {
        var updateClient = _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
        if (updateClient == null) return;

        updateClient.Name = client.Name;
        updateClient.Surname = client.Surname;
        updateClient.NumPassport = client.NumPassport;
        updateClient.Phone = client.Phone;
        updateClient.DateBirthday = client.DateBirthday;
        updateClient.Balance = client.Balance;

        foreach (var oldAccount in updateClient.AccountsClient.ToList())
        {
            if (client.AccountsClient.All(a => a.Id != oldAccount.Id))
            {
                _dbContext.Accounts.Remove(oldAccount);
            }
        }
        
        foreach (var account in client.AccountsClient)
        {
            var existingAccount = updateClient.AccountsClient.FirstOrDefault(a => a.Id == account.Id);
            if (existingAccount != null)
            {
                existingAccount.Amount = account.Amount;
            }
            else
            {
                updateClient.AccountsClient.Add(account);
                account.ClientId = updateClient.Id;
            }
        }
        _dbContext.SaveChanges();
    }

    public void Delete(Guid clientId)
    {
        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
        if (client == null) return;
        _dbContext.Clients.Remove(client);
        
        _dbContext.SaveChanges();
    }
    public void AddAccount(Guid clientId, Account account)
    {
        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
        client?.AccountsClient.Add(account);
        _dbContext.SaveChanges();
    }

    public void DeleteAccount(Guid clientId, Guid accountId)
    {
        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
        var account = client?.AccountsClient.FirstOrDefault(a => a.Id == accountId);
        if (client != null && account != null)
            client.AccountsClient.Remove(account);
        _dbContext.SaveChanges();
    }

    public Client? SearchYoungClient()
    {
        return _dbContext.Clients.MinBy(c => c.Age);
    }
    public Client? SearchOldClient()
    {
        return _dbContext.Clients.MaxBy(c => c.Age);
    }
    public int SearchAverageAgeClient()
    {
        return (int)_dbContext.Clients.Average(c => c.Age);
    }
}