using System.Text.Json;
using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public class StorageReader
{
    public string storagePath;
    
    public StorageReader(string path)
    {
        storagePath = path;
    }

    public AndroidDirectory? ReadStorage()
    {
        using StreamReader r = new(storagePath);
        
        string json = r.ReadToEnd();
        return JsonSerializer.Deserialize<AndroidDirectory>(json);
    }

    public DirectoryElement? SearchSubDirectory(List<DirectoryElement> searchTarget, string pathToFind)
    {
        if (searchTarget.Count == 0)
        {
            return null;
        }

        DirectoryElement? exactFound = searchTarget.Find(file =>
        {
            return file.Path == pathToFind;
        });
        if (exactFound != null)
        {
            return exactFound;
        }

        List<DirectoryElement> filedDescending = searchTarget.OrderByDescending(file => file.Path.Length).ToList();
        foreach (var file in filedDescending)
        {
            if (pathToFind.StartsWith(file.Path))
            {
                return SearchSubDirectory(file.Files, pathToFind);
            }
        }

        return null;
    }
}