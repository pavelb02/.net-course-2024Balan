using System.Globalization;
using System.Text.Json;
using BankSystem.Domain.Models;
using CsvHelper;

namespace ExportEntity;

public class ExportService<T> where T:Person
{
    private string PathToDirectory { get; set; }
    private string CsvFileName { get; set; }

    public ExportService(string pathToDirectory, string csvFileName)
    {
        PathToDirectory = pathToDirectory;
        CsvFileName = csvFileName;
    }

    public ExportService()
    {
    }

    public void WriteItemsToCsv(List<T> items)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }

        var fullPath = Path.Combine(PathToDirectory, CsvFileName);

        using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                using (var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    writer.WriteHeader<T>();
                    writer.NextRecord();

                    foreach (var item in items)
                    {
                        writer.WriteRecord(item);
                        writer.NextRecord();
                    }

                    writer.Flush();
                }
            }
        }
    }

    public List<T> ReadItemsFromCsv()
    {
        string fullPath = Path.Combine(PathToDirectory, CsvFileName);

        using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                using (var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var itemsFromCsv = reader.GetRecords<T>().ToList();

                    return itemsFromCsv;
                }
            }
        }
    }

    public void WriteToJson(List<T> items, string fullPath)
    {
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fileStream, items);
        }
    }
    
    public List<T> ReadItemsFromJson(string fullPath)
    {
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            var items = JsonSerializer.Deserialize<List<T>>(fileStream);
            
            return items;
        }
    }
    
    public void WriteToJson(T item, string fullPath)
    {
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            JsonSerializer.Serialize(fileStream, item);
        }
    }
    
    public T ReadItemFromJson(string fullPath)
    {
        using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
        {
            var item = JsonSerializer.Deserialize<T>(fileStream);

            return item;
        }
    }
}