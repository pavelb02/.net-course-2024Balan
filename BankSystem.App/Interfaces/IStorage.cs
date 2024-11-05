using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, K>
{
    public Task<Guid> AddAsync(T item, CancellationToken cancellationToken);
    public Task<T> GetByIdAsync(Guid itemId, CancellationToken cancellationToken);
    public Task<List<T>> GetCollectionAsync(K searchRequest, CancellationToken cancellationToken);
    public Task<Guid> UpdateAsync(T item, CancellationToken cancellationToken);
    public Task<Guid> DeleteAsync(Guid itemId, CancellationToken cancellationToken);
}