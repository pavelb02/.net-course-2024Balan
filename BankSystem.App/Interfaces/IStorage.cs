using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, K>
{
    public Task<Guid> AddAsync(T item);
    public Task<T> GetByIdAsync(Guid itemId);
    public Task<List<T>> GetCollectionAsync(K searchRequest);
    public Task<Guid> UpdateAsync(T item);
    public Task<Guid> DeleteAsync(Guid itemId);
}