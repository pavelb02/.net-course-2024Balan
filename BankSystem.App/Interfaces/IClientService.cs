using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientService
{
    Task<ClientDto> GetClientAsync(Guid clientId);
    Task DeleteClientAsync(Guid clientId);
    Task DeleteAccountAsync(Guid accountId);
    Task<Guid> AddClientAsync(ClientDto client, string currencyCode);
    Task AddAccountAsync(Guid clientId, string currencyCode);
    Task UpdateClientAsync(Guid clientId, ClientDto newClient);
    Task<List<ClientDto>> FilterClientsAsync(SearchRequest searchRequest);
    //Task<bool> Debit(WithdrawalRequest withdrawalRequest);
}