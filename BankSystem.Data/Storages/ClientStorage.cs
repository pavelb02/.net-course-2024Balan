using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages;

public class ClientStorage
{
    private Dictionary<Client, Account[]> Clients { get; set; }

    public ClientStorage()
    {
        Clients = new Dictionary<Client, Account[]>();
    }
    public Dictionary<Client, Account[]> GetAllClients()
    {
        return new Dictionary<Client, Account[]>(Clients);
    }
    public KeyValuePair<Client, Account[]>? GetClient(Client client)
    {
        if (Clients.TryGetValue(client, out var accounts))
        {
            return new KeyValuePair<Client, Account[]>(client, accounts);
        }
        return null;
    }
    public void UpdateClientAccounts(Client client, Account[] accounts)
    {
        Clients[client] = accounts;
    }
    public void AddClientToCollection(Dictionary<Client, Account[]> clients)
    {
        foreach (var client in clients)
        {
            Clients.Add(client.Key, client.Value);
        }
    }
    public void UpdateCollection(Dictionary<Client, Account[]> clients)
    {
      foreach (var client in clients)
      {
          Clients[client.Key] = client.Value;
      }
    }

    public Client? SearchYoungClient()
    {
        return Clients.MinBy(c => c.Key.Age).Key;
    }
    public Client? SearchOldClient()
    {
        return Clients.MaxBy(c => c.Key.Age).Key;
    }
    public int? SearchAverageAgeClient()
    {
        return (int)Clients.Average(c => c.Key.Age);
    }
}