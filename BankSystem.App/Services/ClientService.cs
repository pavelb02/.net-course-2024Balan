using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Exeptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services;

public class ClientService : IClientService
{
    private readonly IClientStorage _clientStorage;
    private readonly ICurrencyService _currencyService;
    private readonly IMapper _mapper;

    public ClientService(IClientStorage clientStorage, ICurrencyService currencyService, IMapper mapper)
    {
        _clientStorage = clientStorage;
        _currencyService = currencyService;
        _mapper = mapper;
    }

    public async Task<ClientDto> GetClientAsync(Guid clientId)
    {
       var client = await _clientStorage.GetByIdAsync(clientId);
       var clientDto = _mapper.Map<ClientDto>(client);
       return clientDto;
    }
    
    public async Task<Guid> DeleteClientAsync(Guid clientId)
    {
        var deletedId = await _clientStorage.DeleteAsync(clientId);
        return deletedId;
    }
    
    public async Task<Guid> DeleteAccountAsync(Guid accountId)
    {
        var deletedAccountId = await _clientStorage.DeleteAccountAsync(accountId);
        return deletedAccountId;
    }

    public async Task<Guid> AddClientAsync(ClientDto clientDto, string currencyCode)
    {
        try
        {
            if (! await ValidateAddClientAsync(clientDto)) return Guid.Empty;
            var currencyId = await _currencyService.GetCurrencyAsync(currencyCode);
            var account = new Account(clientDto.Id, currencyId);
            clientDto.AccountsClient.Add(account);
            var client = _mapper.Map<Client>(clientDto);
            var clientId = await _clientStorage.AddAsync(client);
            return clientId;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении клиента: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<Guid> AddAccountAsync(Guid clientId, string currencyCode)
    {
        try
        {
            var client = await _clientStorage.GetByIdAsync(clientId);
            var currencyId = await _currencyService.GetCurrencyAsync(currencyCode);
            var account = new Account(client.Id, currencyId);
            var accountId = await _clientStorage.AddAccountAsync(clientId, account);
            return accountId;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при добавлении аккаунта: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<Guid> UpdateClientAsync(Guid clientId, ClientDto newClient)
    {
        try
        {
            var updatedClientId = Guid.Empty;
            if (await ValidateAddClientAsync(newClient))
            {
                var client = await _clientStorage.GetByIdAsync(clientId);
                
                if (!string.IsNullOrWhiteSpace(newClient.Name))
                    client.Name = newClient.Name;

                if (!string.IsNullOrWhiteSpace(newClient.Surname))
                    client.Surname = newClient.Surname;

                if (!string.IsNullOrWhiteSpace(newClient.NumPassport))
                    client.NumPassport = newClient.NumPassport;

                if (newClient.DateBirthday != default)
                    client.DateBirthday = newClient.DateBirthday;

                if (!string.IsNullOrWhiteSpace(newClient.Phone))
                    client.Phone = newClient.Phone;
                
                foreach (var newAccount in newClient.AccountsClient)
                {
                    if (client.AccountsClient.All(a => a.Id != newAccount.Id))
                    {
                        client.AccountsClient.Add(newAccount);
                    }
                }

                updatedClientId = await _clientStorage.UpdateAsync(_mapper.Map<Client>(newClient));
            }
            return updatedClientId;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Ошибка при обновлении клиента: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<List<ClientDto>> FilterClientsAsync(SearchRequest searchRequest)
    {
        var filteredClients = await _clientStorage.GetCollectionAsync(searchRequest);
        var filteredClientsDto = _mapper.Map<List<ClientDto>>(filteredClients);
        return filteredClientsDto;
    }

    public async Task<bool> Debit(WithdrawalRequest withdrawalRequest)
    {
        var client = await _clientStorage.GetByIdAsync(withdrawalRequest.ClientId);
        var currencyId = await _currencyService.GetCurrencyAsync(withdrawalRequest.CurrencyCode);
        var account = client.AccountsClient.FirstOrDefault(a => a.CurrencyId == currencyId);
        if (account == null)
        {
            Console.WriteLine($"Аккаунт клиента с валютой {withdrawalRequest.CurrencyCode} не найден.");
            return false;
        }

        if (account.Amount < withdrawalRequest.WithdrawalAmount)
        {
            Console.WriteLine("Недостаточно средств на счете.");
            return false;
        }
        account.Amount -= withdrawalRequest.WithdrawalAmount;
        
        await _clientStorage.UpdateAsync(client);
        return true;
    }

    private static Task<bool> ValidateAddClientAsync(ClientDto client)
    {
        if (string.IsNullOrWhiteSpace(client.Name))
        {
            throw new ArgumentException("Имя не может быть null, пустым или состоять только из пробелов.", nameof(client.Name));
        }

        if (string.IsNullOrWhiteSpace(client.Name))
        {
            throw new ArgumentException("Фамиилия не может быть null, пустой или состоять только из пробелов.", nameof(client.Surname));
        }

        if (string.IsNullOrWhiteSpace(client.NumPassport))
        {
            throw new ArgumentException("Номер паспорта не может быть null, пустым или состоять только из пробелов.", nameof(client.NumPassport));
        }
        var dateNow = DateTime.Now;
        int age = dateNow.Year - client.DateBirthday.Year;
        if (dateNow.DayOfYear < client.DateBirthday.DayOfYear)
        {
            age--;
        }
        if (age < 18)
        {
            throw new PersonAgeException(age);
        }

        if (age <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст должен быть положительным.");
        }

        return Task.FromResult(true);
    }
}