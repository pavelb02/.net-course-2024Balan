using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IClientService
{
    Task<ClientDto> GetClientAsync(Guid clientId, CancellationToken cancellationToken);
    Task<Guid> DeleteClientAsync(Guid clientId, CancellationToken cancellationToken);
    Task<Guid> DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<Guid> AddClientAsync(ClientDto client, string currencyCode, CancellationToken cancellationToken);
    Task<Guid> AddAccountAsync(Guid clientId, string currencyCode, CancellationToken cancellationToken);
    Task<Guid> UpdateClientAsync(Guid clientId, ClientDto newClient, CancellationToken cancellationToken);
    Task<List<ClientDto>> FilterClientsAsync(SearchRequest searchRequest, CancellationToken cancellationToken);
    //Task<bool> Debit(WithdrawalRequest withdrawalRequest, CancellationToken cancellationToken);
}