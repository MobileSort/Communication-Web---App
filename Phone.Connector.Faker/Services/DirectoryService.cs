using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public class DirectoryService
{
    public string storagePath;
    public AndroidDirectory readDirectory;

    public DirectoryService(string path)
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

        DirectoryElement? exactFound = searchTarget.Find(file => { return file.Path == pathToFind; });
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

    public bool RemoveItem(string pathToRemove)
    {
        var elementToMove = SearchSubDirectory(readDirectory.Directories, pathToRemove);
        if (elementToMove == null)
        {
            return false;
        }

        return DeleteDirectory(elementToMove);
    }

    public bool AddDirectory(DirectoryElement directoryToAdd)
    {
        //Create validation to clean the possible file at the end of the path
        var alreadlyExists = SearchSubDirectory(readDirectory.Directories, directoryToAdd.Path);
        if (alreadlyExists != null)
        {
            return true;
        }

        if (!directoryToAdd.Path.Contains(readDirectory.Path))
        {
            directoryToAdd.Path = readDirectory.Path + directoryToAdd.Path;
        }

        DirectoryElement? startDirectory = FindRootDirectory(directoryToAdd.Path);

        if (startDirectory == null)
        {
            var pathWORoot = directoryToAdd.Path.Replace(readDirectory.Path, "");
            var currentToAddDirPath = readDirectory.Path + "/" + pathWORoot.Split("/")[1];
            readDirectory.Directories.Add(new(currentToAddDirPath, "directory", 0, []));
            startDirectory = FindRootDirectory(currentToAddDirPath);
        }

        if (startDirectory == null)
        {
            throw new Exception("There was a problem on creating the folder");
        }

        var leaf = FindLeaf(startDirectory, directoryToAdd.Path);
        return AddDirectoryRecursively(leaf, directoryToAdd);
    }

    public bool AddFile(DirectoryElement element)
    {
        if (!element.Path.Contains(readDirectory.Path))
        {
            element.Path = readDirectory.Path + element.Path;
            readDirectory.Directories.Add(element);
            return true;
        }

        var lastElement = element.Path.Split("/").Last();
        var lastDir = element.Path.Replace("/" + lastElement, "");
        DirectoryElement directoriesToAdd = new(lastDir, "directory", 0, []);
        AddDirectory(directoriesToAdd);
        var addedDir = SearchSubDirectory(readDirectory.Directories, directoriesToAdd.Path);
        if (addedDir == null)
        {
            return false;
        }

        addedDir.Files.Add(element);
        return true;
    }

    public bool MoveItem(string toMove, string destination)
    {
        var elementToMove = SearchSubDirectory(readDirectory.Directories, toMove);
        var folderToMoveTo = SearchSubDirectory(readDirectory.Directories, destination);
        if (elementToMove == null)
        {
            return false;
        }

        if (folderToMoveTo == null)
        {
            AddDirectory(new DirectoryElement(destination, "directory", 0, []));
            folderToMoveTo = SearchSubDirectory(readDirectory.Directories, destination);

            if (folderToMoveTo == null)
            {
                throw new Exception("Problem while creating the folder");
            }
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

    private bool AddDirectoryRecursively(DirectoryElement toBeAddedTo, DirectoryElement directoryToAdd)
    {
        if (toBeAddedTo.Path == directoryToAdd.Path)
        {
            return true;
        }

        if (toBeAddedTo.Files.Contains(directoryToAdd))
        {
            return true;
        }

        var pathWORoot = directoryToAdd.Path.Replace(toBeAddedTo.Path, "");
        var currentToAddDirPath = toBeAddedTo.Path + "/" + pathWORoot.Split("/")[1];
        toBeAddedTo.Files.Add(new(currentToAddDirPath, "directory", 0, []));
        var leaf = FindLeaf(toBeAddedTo, directoryToAdd.Path);
        return AddDirectoryRecursively(leaf, directoryToAdd);
    }

    private DirectoryElement? FindRootDirectory(string pathToFind)
    {
        List<DirectoryElement> filedDescending =
            readDirectory.Directories.OrderByDescending(file => file.Path.Length).ToList();
        foreach (var file in filedDescending)
        {
            if (pathToFind.StartsWith(file.Path))
            {
                return file;
            }
        }

        return null;
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

    private DirectoryElement FindLeaf(DirectoryElement searchTarget, string pathToFind)
    {
        if (searchTarget.Files == null || searchTarget.Files.Count == 0)
        {
            return searchTarget;
        }

        List<DirectoryElement> filedDescending =
            searchTarget.Files.OrderByDescending(file => file.Path.Length).ToList();
        foreach (var file in filedDescending)
        {
            if (pathToFind.StartsWith(file.Path))
            {
                return FindLeaf(file, pathToFind);
            }
        }

        return searchTarget;
    }

    public List<DirectoryElement> SearchItem(string searchTerm, List<DirectoryElement> directories)
    {
        List<DirectoryElement> results = new List<DirectoryElement>();

        foreach (var directory in directories)
        {
            if (directory.Path.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(directory);
            }

            if (directory.Files != null)
            {
                results.AddRange(SearchItem(searchTerm, directory.Files));
            }
        }

        return results;
    }

    public bool MoveItemsOnDirectoryByTags(string targetDirectoryPath, List<Tag> tags, string directoryDestinationPath)
    {
        var destinationDirectory = SearchSubDirectory(readDirectory.Directories, directoryDestinationPath);
        if (destinationDirectory == null)
        {
            AddDirectory(new DirectoryElement(directoryDestinationPath, "directory", 0, []));
            destinationDirectory = SearchSubDirectory(readDirectory.Directories, directoryDestinationPath);

            if (destinationDirectory == null)
            {
                throw new Exception("Problem while creating the folder");
            }
        }

        var targetDirectory = SearchSubDirectory(readDirectory.Directories, targetDirectoryPath);
        if (targetDirectory == null)
        {
            return false;
        }

        if (targetDirectory.Files.Count == 0)
        {
            return true;
        }

        TagService tagService = new();
        var targetDirectoryFiles = new List<DirectoryElement>(targetDirectory.Files);
        foreach (var file in targetDirectoryFiles)
        {
            bool matches = true;
            foreach (var tag in tags)
            {
                if (!matches)
                {
                    break;
                }

                var typeTag = tagService.GetTypeTagById(tag.TypeTag);
                if (typeTag == null)
                {
                    matches = false;
                    continue;
                }

                if (!tagService.MatchTagValue(typeTag, tag.ValueTag, file))
                {
                    matches = false;
                    continue;
                }
            }

            if (!matches)
            {
                continue;
            }

            var succesfullyMoved = MoveItem(file.Path, directoryDestinationPath);
            if (!succesfullyMoved)
            {
                return false;
            }
        }

        return true;
    }
}