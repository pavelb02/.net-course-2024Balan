using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace ExportEntity.Tests;

public class ExportServiceTests
{
    private ClientStorage _clientStorage;
    private ClientService _clientService;
    private CurrencyService _currencyService;
    private CurrencyStorage _currencyStorage;

    public ExportServiceTests()
    {
        _currencyStorage = new CurrencyStorage();
        _currencyService = new CurrencyService(_currencyStorage);
        _clientStorage = new ClientStorage();
        _clientService = new ClientService(_clientStorage, _currencyService);
    }
    [Fact]
    public void WriteClientsToCsvAndReadFromCsvTest()
    {
        //Arrange
        List<Client> clientsFromDb = new (_clientService.FilterClients(new SearchRequest()));
        string pathToDirectory = Path.Combine("D:", "Программирование","Dex backend 2024", "Practice",".net-course-2024Balan", "Tool");
        string fileName = "clients.csv";
        ExportService exportService = new ExportService(pathToDirectory, fileName);
        
        //Act
        exportService.WriteClientsToCsv(clientsFromDb);
        List<Client> clientsFromFile = exportService.ReadClientsFromCsv();
        
        //Assert
        Assert.Equal(clientsFromDb, clientsFromFile);
    }
    [Fact]
    public void ReadClientsFromCsvAndWriteToDbTest()
    {
        //Arrange
        string pathToDirectory = Path.Combine("D:", "Программирование", "Dex backend 2024", "Practice", ".net-course-2024Balan", "Tool");
        string fileName = "clients.csv";
        ExportService exportService = new ExportService(pathToDirectory, fileName);
        List<Client> clientsFromFile = exportService.ReadClientsFromCsv();
        
        //Act
        foreach (var client in clientsFromFile)
        {
            _clientService.AddClients(client, "USD");
        }
        List<Client> clientsFromDb = new List<Client>();
        foreach (var client in clientsFromFile)
        {
            clientsFromDb.Add(_clientService.GetClient(client.Id));
        }

        //Assert
        Assert.Equal(clientsFromFile, clientsFromDb);
    }
}