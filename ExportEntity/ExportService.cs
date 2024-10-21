using System.Globalization;
using BankSystem.Domain.Models;
using CsvHelper;

namespace ExportEntity;

public class ExportService
{
    private string _pathToDirectory { get; set; }
    private string _csvFileName { get; set; }

    public ExportService(string pathToDirectory, string csvFileName)
    {
        _pathToDirectory = pathToDirectory;
        _csvFileName = csvFileName;
    }
    
    public void WriteClientsToCsv(List<Client> clients)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }
        string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
        
        using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                using (var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    writer.WriteField(nameof(Client.Id));
                    writer.WriteField(nameof(Client.Name));
                    writer.WriteField(nameof(Client.Surname));
                    writer.WriteField(nameof(Client.NumPassport));
                    writer.WriteField(nameof(Client.Phone));
                    writer.WriteField(nameof(Client.DateBirthday));
    
                    writer.NextRecord();

                    foreach (var client in clients)
                    {
                        writer.WriteField(client.Id);
                        writer.WriteField(client.Name);
                        writer.WriteField(client.Surname);
                        writer.WriteField(client.NumPassport);
                        writer.WriteField(client.Phone);
                        writer.WriteField(client.DateBirthday.ToString("yyyy-MM-dd"));
        
                        writer.NextRecord();
                    }
                    writer.Flush();
                }
            }
        }
    }
    public List<Client> ReadClientsFromCsv()
    {
        string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
        List<Client> clientsFromCsv = new List<Client>();
        
        using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                using (var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    // 1 вариант
                    reader.Context.RegisterClassMap<ClientMap>();
                    clientsFromCsv = reader.GetRecords<Client>().ToList();

                    // 2 вариант
                    /*
                    reader.Read();
                    reader.ReadHeader();
                    while (reader.Read())
                    {
                        clientsFromCsv.Add(reader.GetRecord<Client>());
                    }
                    */
                    return clientsFromCsv;
                }
            }
        }
    }
}