using BankSystem.App.Services;

namespace BankSystem.App.Interfaces;

public interface IStorage<T>
{
    public void Add(T item);
    public List<T> Get(SearchRequest searchRequest);
    public void Update(T item);
    public void Delete(T item);
}