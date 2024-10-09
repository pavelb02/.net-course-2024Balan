using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces;

public interface IEmployeeStorage : IStorage<Employee, SearchRequest> { }