using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.Domain.Models;

namespace BankSystem.App.Mapping;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();

        CreateMap<ClientDto, Client>();
    }
}