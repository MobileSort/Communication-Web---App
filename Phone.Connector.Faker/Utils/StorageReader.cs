using System.Text.Json;
using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public class StorageReader
{
    public string storagePath;
    public AndroidDirectory readDirectory;
    
    public StorageReader(string path)
    {
        storagePath = path;
        using StreamReader r = new(storagePath);
        
        string json = r.ReadToEnd();
        var foundStorage = JsonSerializer.Deserialize<AndroidDirectory>(json);
        if (foundStorage == null)
        {
            throw new Exception("Not Found");
        }
        readDirectory = foundStorage;
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

    public bool MoveItem(string toMove, string detination)
    {
        
        var elementToMove = SearchSubDirectory(readDirectory.Directories, toMove);
        var folderToMoveTo = SearchSubDirectory(readDirectory.Directories, detination);
        if (elementToMove == null || folderToMoveTo == null)
        {
            return false;
        }
        if (folderToMoveTo.Type == "file")
        {
            return false;
        }
        if (!DeleteDirectory(elementToMove))
        {
            return false;
        }

        var filename = elementToMove.Path.Split("/").Last();
        elementToMove.Path = folderToMoveTo.Path + "/" + filename;
        folderToMoveTo.Files.Add(elementToMove);
        return true;
    }

    public bool WriteChanges()
    {
        try
        {
            var updatedStorage = JsonSerializer.Serialize<AndroidDirectory>(readDirectory);
            using var streamWriter = new StreamWriter(storagePath, false);
            streamWriter.Write(updatedStorage);
        }
        catch
        {
            return false;
        }
        return true;
    }
    
    private bool DeleteDirectory(DirectoryElement file)
    {
        var parentDirectory = FindParentDirectory(file);
        if (parentDirectory == null)
        {
            return false;
        }
        parentDirectory.Files.Remove(file);
        return true;
    }
    
    private DirectoryElement? FindParentDirectory(DirectoryElement file)
    {
        var parent = readDirectory.Directories.FirstOrDefault(d => d.Files.Contains(file));
        if (parent != null)
        {
            return parent;
        }

        foreach (var directory in readDirectory.Directories)
        {
            parent = FindParentDirectoryRecursive(directory, file);
            if (parent != null)
            {
                return parent;
            }
        }

        return null;
    }
    
    private DirectoryElement? FindParentDirectoryRecursive(DirectoryElement directory, DirectoryElement file)
    {
        if (directory.Files.Contains(file))
        {
            return directory;
        }

        foreach (var childDirectory in directory.Files.Where(f => f.Type == "directory"))
        {
            var parent = FindParentDirectoryRecursive(childDirectory, file);
            if (parent != null)
            {
                return parent;
            }
        }

        return null;
    }
}