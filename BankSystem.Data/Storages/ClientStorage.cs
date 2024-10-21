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
        if (_dbContext.Clients.Any(c => c.Id == client.Id))
        {
            throw new InvalidOperationException($"Клиент с ID {client.Id} уже существует.");
        }
        _dbContext.Clients.Add(client);
        _dbContext.SaveChanges();
    }

    public Client GetById(Guid clientId)
    {
        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
        if (client == null) throw new ArgumentException($"Клиент с Id {clientId} не найден.");
        return client;
    }

    public List<Client> GetCollection(SearchRequest searchRequest)
    {
        IQueryable<Client> request = _dbContext.Clients;
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

        //var countRecords = request.Count();
        if (searchRequest.PageSize != 0 && searchRequest.PageNumber != 0)
        {
            var clients = request
                .OrderBy(c => c.Surname).ThenBy(c => c.Name)
                .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToList();
            return clients;
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
        
        client.AccountsClient.Add(account);
        _dbContext.SaveChanges();
    }

    public void DeleteAccount(Guid accountId)
    {
        var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);
        if (account != null)
        {
            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();
        }
        else
        {
            throw new ArgumentException($"Аккаунт с Id {accountId} не найден.");
        }
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