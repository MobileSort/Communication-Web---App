using System.Text.Json.Serialization;

namespace Phone.Connector.Faker.Models.ViewModel;

public class InternalConfig
{
    [JsonPropertyName("tags")] public List<Tag> Tags { get; set; }
    [JsonPropertyName("typeTags")] public List<TypeTag> TypeTags { get; set; }
    [JsonPropertyName("ordering")] public List<Tag> Orderings { get; set; }
}

public class Ordering
{
    [JsonPropertyName("idOrdering")] public int IdOrdering { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("tags")] public List<int> Tags { get; set; }
}

public class Tag
{
    [JsonPropertyName("idTag")] public int IdTag { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("color")] public string Color { get; set; }
    [JsonPropertyName("typeTag")] public int TypeTag { get; set; }
    [JsonPropertyName("valueTag")] public string ValueTag { get; set; }

    public Tag(int idTag, string name, string color, int typeTag, string valueTag)
    {
        IdTag = idTag;
        Name = name;
        Color = color;
        TypeTag = typeTag;
        ValueTag = valueTag;
    }
}

public class TypeTag
{
    [JsonPropertyName("idTypeTag")] public int IdTypeTag { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }

    public TypeTag(int idTypeTag, string description)
    {
        IdTypeTag = idTypeTag;
        Description = description;
    }
}