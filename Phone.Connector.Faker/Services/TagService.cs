using System.Text.Json;
using System.Xml.Linq;
using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public class TagService
{
  public TagService()
  {
  }

  public List<Tag> ListTags()
  {
    InternalConfig config = InternalConfigSingle.internalConfigRead;
    return config.Tags;
  }
  public Tag? GetTagById(int id)
  {
    var tags = ListTags();
    if (tags.Count == 0)
    {
      return null;
    }

    Tag? foundTag = tags.Find((tag) => tag.IdTag == id);
    return foundTag;
  }
  public int? AddTag(string Name, string Color, int TypeTag, string ValueTag)
  {
    TypeTag? foundTagType = GetTypeTagById(TypeTag);
    if (foundTagType == null)
    {
      return null;
    }

    int lastId = 0;
    foreach (var tag in InternalConfigSingle.internalConfigRead.Tags)
    {
      if (tag.IdTag > lastId)
      {
        lastId = tag.IdTag;
      }
    }

    Tag tagToAdd = new(lastId + 1, Name, Color, TypeTag, ValueTag);
    InternalConfigSingle.internalConfigRead.Tags.Add(tagToAdd);
    return tagToAdd.IdTag;
  }

  public bool RemoveTag(int id)
  {
    var foundTag = GetTagById(id);
    if (foundTag == null)
    {
      return false;
    }
    return InternalConfigSingle.internalConfigRead.Tags.Remove(foundTag);
  }
  public List<TypeTag> ListTypeTags()
  {
    return InternalConfigSingle.internalConfigRead.TypeTags;
  }
  public TypeTag? GetTypeTagById(int id)
  {
    var tags = ListTypeTags();
    if (tags.Count == 0)
    {
      return null;
    }

    TypeTag? foundTypeTag = tags.Find((tag) => tag.IdTypeTag == id);
    return foundTypeTag;
  }
  public int? AddTypeTag(string description)
  {
    int lastId = 0;
    foreach (var tag in InternalConfigSingle.internalConfigRead.TypeTags)
    {
      if (tag.IdTypeTag > lastId)
      {
        lastId = tag.IdTypeTag;
      }
    }

    TypeTag tagToAdd = new(lastId + 1, description);
    InternalConfigSingle.internalConfigRead.TypeTags.Add(tagToAdd);
    return tagToAdd.IdTypeTag;
  }
  public bool MatchTagValue(TypeTag typeTag, string valueToCheckFor, DirectoryElement item)
  {
    if(typeTag.IdTypeTag == 14)//file extention
    {
      var lastElement = item.Path.Split("/").Last();
      var extention = "."+lastElement.Split(".").Last();
      return extention.Equals(valueToCheckFor);
    }
    if (typeTag.IdTypeTag == 15)//file name
    {
      var lastElement = item.Path.Split("/").Last();
      return lastElement.Contains(valueToCheckFor);
    }
    return false;
  }
  public bool WriteChanges()
  {
    return InternalConfigSingle.WriteChanges();
  }
  public bool RemoveTypeTag(int id)
  {
    var foundTag = GetTypeTagById(id);
    if (foundTag == null)
    {
      return false;
    }
    return InternalConfigSingle.internalConfigRead.TypeTags.Remove(foundTag);
  }
}