using System.Text.Json.Serialization;

namespace Phone.Connector.Faker.Models.View;

public class AddOrderingRequest
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("tags")] public List<int> Tags { get; set; }
    [JsonPropertyName("directoryDestination")] public string DirectoryDestination { get; set; }
}