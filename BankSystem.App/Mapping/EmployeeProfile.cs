using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.Domain.Models;

namespace BankSystem.App.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>();

        CreateMap<EmployeeDto, Employee>();
    }
}