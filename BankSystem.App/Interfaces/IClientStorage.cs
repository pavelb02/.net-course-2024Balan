using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientStorage : IStorage<Client, SearchRequest>
{
    public void AddAccount(Guid clientId, Account account);
    public void DeleteAccount(Guid clientId, Guid accountId);
} 