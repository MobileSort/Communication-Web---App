using System.Text.Json;
using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public class TagService
{
    public string internalConfigPath { get; set; }
    public InternalConfig internalConfigRead { get; set; }

    public TagService(string path)
    {
        internalConfigPath = path;
        using StreamReader r = new(internalConfigPath);

        string json = r.ReadToEnd();
        var foundConfig = JsonSerializer.Deserialize<InternalConfig>(json);
        if (foundConfig == null)
        {
            throw new Exception("Not Found");
        }

        internalConfigRead = foundConfig;
    }

    public List<Tag> ListTags()
    {
        return internalConfigRead.Tags;
    }

    public int? AddTag(string Name, string Color, int TypeTag,string ValueTag)
    {
        TypeTag? foundTagType = GetTagTypeByID(TypeTag);
        if (foundTagType == null)
        {
            return null;
        }

        int lastId = 0;
        foreach (var tag in internalConfigRead.Tags)
        {
            if (tag.IdTag > lastId)
            {
                lastId = tag.IdTag;
            }
        }

        Tag tagToAdd = new(lastId+1, Name, Color, TypeTag, ValueTag);
        internalConfigRead.Tags.Add(tagToAdd);
        return tagToAdd.IdTag;
    }

    public List<TypeTag> ListTypeTags()
    {
        return internalConfigRead.TypeTags;
    }

    public bool AddTypeTag(TypeTag tagToAdd)
    {
        internalConfigRead.TypeTags.Add(tagToAdd);
        return true;
    }

    public Tag? GetTagByID(int id)
    {
        var tags = ListTags();
        if (tags.Count == 0)
        {
            return null;
        }

        Tag? foundTag = tags.Find((tag) => { return tag.IdTag == id; });
        return foundTag;
    }

    public TypeTag? GetTagTypeByID(int id)
    {
        var tags = ListTypeTags();
        if (tags.Count == 0)
        {
            return null;
        }

        TypeTag? foundTypeTag = tags.Find((tag) => { return tag.IdTypeTag == id; });
        return foundTypeTag;
    }
    
    public bool WriteChanges()
    {
        try
        {
            var updatedStorage = JsonSerializer.Serialize<InternalConfig>(internalConfigRead);
            using var streamWriter = new StreamWriter(internalConfigPath, false);
            streamWriter.Write(updatedStorage);
        }
        catch
        {
            return false;
        }
        return true;
    }
}