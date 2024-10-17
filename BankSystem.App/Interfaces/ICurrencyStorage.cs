using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyStorage
{
    public Guid Get(string currencyCode);
    public void Add(Currency currency);
    public void Delete(string currencyCode);
}