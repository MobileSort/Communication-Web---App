using System.Text.Json.Serialization;

namespace Phone.Connector.Faker.Models.ViewModel;

public class AndroidDirectory
{
    [JsonPropertyName("emulated")] public bool Emulated { get; set; }

    [JsonPropertyName("removable")] public bool Removable { get; set; }

    [JsonPropertyName("path")] public string Path { get; set; }

    [JsonPropertyName("uuid")] public Guid Uuid { get; set; }

    [JsonPropertyName("fs_type")] public string FsType { get; set; }

    [JsonPropertyName("total_bytes")] public long TotalBytes { get; set; }

    [JsonPropertyName("available_bytes")] public long AvailableBytes { get; set; }

    [JsonPropertyName("system_bytes")] public long SystemBytes { get; set; }

    [JsonPropertyName("data_bytes")] public long DataBytes { get; set; }

    [JsonPropertyName("read_only")] public bool ReadOnly { get; set; }

    [JsonPropertyName("primary")] public bool Primary { get; set; }

    [JsonPropertyName("directories")] public List<DirectoryElement> Directories { get; set; }
}

public class DirectoryElement
{
    [JsonPropertyName("path")] public string Path { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("size_bytes")] public long SizeBytes { get; set; }

    [JsonPropertyName("files")] public List<DirectoryElement> Files { get; set; }

    public DirectoryElement(string path, string type, long sizeBytes, List<DirectoryElement> files)
    {
        Path = path;
        Type = type;
        SizeBytes = sizeBytes;
        Files = files;
    }
}