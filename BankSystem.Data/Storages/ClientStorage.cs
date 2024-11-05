using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storages;

public class ClientStorage : IClientStorage
{
    private BankSystemDbContext _dbContext;

    public ClientStorage()
    {
        _dbContext = new BankSystemDbContext();
    }

    public async Task<Guid> AddAsync(Client client, CancellationToken cancellationToken)
    {
        if (await _dbContext.Clients.AnyAsync(c => c.Id == client.Id))
        {
            throw new InvalidOperationException($"Клиент с ID {client.Id} уже существует.");
        }
        
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
        
        return client.Id;
    }

    public async Task<Client> GetByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        if (client == null)
        {
            throw new ArgumentException($"Клиент с Id {clientId} не найден.");
        }
        
        return client;
    }

    public async Task<List<Client>> GetCollectionAsync(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        IQueryable<Client> request = _dbContext.Clients.Include(c => c.AccountsClient);
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
            return await request
                .OrderBy(c => c.Surname).ThenBy(c => c.Name)
                .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToListAsync();
        }

        return await request.ToListAsync();
    }

    public async Task<Guid> UpdateAsync(Client client, CancellationToken cancellationToken)
    {
        _dbContext.Entry(client).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        
        return client.Id;
    }

    public async Task<Guid> DeleteAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        if (client == null)
        {
            return Guid.Empty;
        }
        
        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();
        
        return client.Id;
    }
    public async Task<Guid> AddAccountAsync(Guid clientId, Account account, CancellationToken cancellationToken)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        if (client == null)
        {
            throw new ArgumentException($"Клиент с Id {clientId} не найден.");
        }
        
        client.AccountsClient.Add(account);
        await _dbContext.SaveChangesAsync();
        
        return client.Id;
    }

    public async Task<Guid> DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        if (account == null)
        {
            throw new ArgumentException($"Аккаунт с Id {accountId} не найден.");
        }
        
        _dbContext.Accounts.Remove(account);
        await _dbContext.SaveChangesAsync();
        
        return account.Id;
    }

    public async Task<Client?> SearchYoungClientAsync()
    {
        return await _dbContext.Clients.OrderBy(c => c.DateBirthday).FirstOrDefaultAsync();
    }

    public async Task<Client?> SearchOldClientAsync()
    {
        return await _dbContext.Clients.OrderByDescending(c => c.DateBirthday).FirstOrDefaultAsync();
    }

    public async Task<int> SearchAverageAgeClientAsync()
    {
        var dateNow = DateTime.Now;
        return (int) await _dbContext.Clients.AverageAsync(c => dateNow.Year - c.DateBirthday.Year -
                                                                (dateNow.DayOfYear < c.DateBirthday.DayOfYear ? 1 : 0));
    }
}