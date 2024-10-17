using BankSystem.App.Services;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, K>
{
    public void Add(T item);
    public T GetById(Guid itemId);
    public List<T> GetCollection(K searchRequest);
    public void Update(Guid itemId, T item);
    public void Delete(Guid itemId);
}