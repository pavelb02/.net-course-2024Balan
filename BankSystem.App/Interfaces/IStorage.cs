using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, K>
{
    public Task AddAsync(T item);
    public Task<T> GetByIdAsync(Guid itemId);
    public Task<List<T>> GetCollectionAsync(K searchRequest);
    public Task UpdateAsync(T item);
    public Task DeleteAsync(Guid itemId);
}