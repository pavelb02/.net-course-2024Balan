using BankSystem.App.Dto;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface ICurrencyService
{
    public Task<Guid> GetCurrencyAsync(string currencyCode, CancellationToken cancellationToken);
    public Task<string> AddCurrencyAsync(CurrencyDto currencyDto, CancellationToken cancellationToken);
    public Task<string> DeleteCurrencyAsync(string currencyCode, CancellationToken cancellationToken);
    public Task<decimal> CurrencyConversionAsync(ConversionCurrencyRequest conversionCurrencyRequest, CancellationToken cancellationToken);
}