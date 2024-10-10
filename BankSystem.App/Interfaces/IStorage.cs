using BankSystem.App.Services;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, K>
{
    public void Add(T item);
    public List<T> Get(K searchRequest);
    public void Update(T item);
    public void Delete(T item);
}