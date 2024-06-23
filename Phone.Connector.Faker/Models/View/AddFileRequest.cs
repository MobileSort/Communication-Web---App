using System.Text.Json.Serialization;
using Phone.Connector.Faker.Models.ViewModel;

public class AddFileRequest
{
    [JsonPropertyName("path")] 
    public string Path { get; set; }
    [JsonPropertyName("size_bytes")] 
    public const long SizeBytes = 0;
}