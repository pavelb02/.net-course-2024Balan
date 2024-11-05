using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientStorage : IStorage<Client, SearchRequest>
{
    public Task<Guid> AddAccountAsync(Guid clientId, Account account, CancellationToken cancellationToken);
    public Task<Guid> DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken);
} 