using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage
{
    private Dictionary<Client, Account[]> CollectionClients { get; set; }

    public ClientStorage()
    {
        CollectionClients = new Dictionary<Client, Account[]>();
    }
    public Dictionary<Client, Account[]> GetAllClients()
    {
        return CollectionClients;
    }
    public void AddClientToCollection(Dictionary<Client, Account[]> collectionClients)
    {
        foreach (var client in collectionClients)
        {
            CollectionClients.Add(client.Key, client.Value);
        }
    }
    public Client? SearchYoungClient()
    {
        return CollectionClients.MinBy(c => c.Key.Age).Key;
    }
    public Client? SearchOldClient()
    {
        return CollectionClients.MaxBy(c => c.Key.Age).Key;
    }
    public int? SearchAverageAgeClient()
    {
        return (int)CollectionClients.Average(c => c.Key.Age);
    }
}