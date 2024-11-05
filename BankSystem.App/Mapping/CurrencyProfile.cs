using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.Domain.Models;

namespace BankSystem.App.Mapping;

public class CurrencyProfile : Profile
{
    public CurrencyProfile()
    {
        CreateMap<Currency, CurrencyDto>();

        CreateMap<CurrencyDto, Currency>();
    }
}