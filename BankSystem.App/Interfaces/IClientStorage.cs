using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientStorage : IStorage<Client, SearchRequest>
{
    public Task AddAccountAsync(Guid clientId, Account account);
    public Task DeleteAccountAsync(Guid accountId);
    public Task UpdateAccountAsync(Guid accountId, Account account);
} 