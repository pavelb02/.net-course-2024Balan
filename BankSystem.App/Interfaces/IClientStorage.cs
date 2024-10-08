using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientStorage : IStorage<Client>
{
    public void AddAccount(Client client, Account[] accounts);
    public Account GetAccount(Client client, string currencyCode);
    public void UpdateAccount(Client client, decimal ammount, string currencyCode);
    public void DeleteAccount(Client client, Account account);
} 