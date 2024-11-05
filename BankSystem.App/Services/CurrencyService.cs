using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Newtonsoft.Json;

namespace BankSystem.App.Services;

public class CurrencyService : ICurrencyService
{
    private ICurrencyStorage _currencyStorage;
    private IMapper _mapper;

    public CurrencyService(ICurrencyStorage currencyStorage, IMapper mapper)
    {
        _currencyStorage = currencyStorage;
        _mapper = mapper;
    }
    public async Task<Guid> GetCurrencyAsync(string currencyCode, CancellationToken cancellationToken)
    {
        return await _currencyStorage.GetAsync(currencyCode, cancellationToken);
    }

    public async Task<string> AddCurrencyAsync(CurrencyDto currencyDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!await ValidateCurrencyAsync(currencyDto))
            {
                return "---";
            }

            var currency = _mapper.Map<Currency>(currencyDto);
            var addedCurrencyCode = await _currencyStorage.AddAsync(currency, cancellationToken);
            
            return addedCurrencyCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}\nТрассировка стека: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<string> DeleteCurrencyAsync(string currencyCode, CancellationToken cancellationToken)
    {
        var deletedCurrencyCode = await _currencyStorage.DeleteAsync(currencyCode, cancellationToken);
        return deletedCurrencyCode;
    }

    public async Task<decimal> CurrencyConversionAsync(ConversionCurrencyRequest conversionCurrencyRequest, CancellationToken cancellationToken)
    {
        var apiKey = "Agzu92FAcSfiRD5su5AS3etFazvf3L";
        using (var client = new HttpClient())
        {
            HttpResponseMessage responseMessage = await client.GetAsync(
                $"https://www.amdoren.com/api/currency.php?api_key={apiKey}&from={conversionCurrencyRequest.CurrencyFrom}&to={conversionCurrencyRequest.CurrencyTo}&amount={conversionCurrencyRequest.Amount}");
            
            responseMessage.EnsureSuccessStatusCode();

            string message = await responseMessage.Content.ReadAsStringAsync();
            
            var response = JsonConvert.DeserializeObject<CurrencyResponse>(message);
            
            if (response.Error != 0)
            {
                throw new Exception($"Error in currency conversion: {response.ErrorMessage}");
            }

            return response.Amount;
        }
    }

    private static Task<bool> ValidateCurrencyAsync(CurrencyDto currency)
    {
        if (string.IsNullOrWhiteSpace(currency.Code) || string.IsNullOrWhiteSpace(currency.Name) || string.IsNullOrWhiteSpace(currency.Symbol))
        {
            throw new ArgumentException("В метод передана пустая строка (или из пробелов) или null", nameof(currency));
        }

        if (currency.ExchangeRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currency.ExchangeRate), "Стоимость валюты не может быть не положительной.");
        }
        
        return Task.FromResult(true);
    }
}