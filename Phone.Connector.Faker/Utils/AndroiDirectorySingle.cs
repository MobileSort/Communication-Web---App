using Phone.Connector.Faker.Models.ViewModel;
using System.IO;
using System.Text.Json;

namespace Phone.Connector.Faker.Utils
{
  public class AndroiDirectorySingle
  {
    private static AndroidDirectory? _readDirectory;
    public static AndroidDirectory readDirectory
    {
      get
      {
        if (_readDirectory == null)
        {
          using StreamReader r = new(Constants.StoragePath);

          string json = r.ReadToEnd();
          var foundStorage = JsonSerializer.Deserialize<AndroidDirectory>(json);
          if (foundStorage == null)
          {
            throw new Exception("Not Found");
          }
          _readDirectory = foundStorage;
        }

        return _readDirectory;
      }
      set
      {
        _readDirectory = value;
      }
    }

    public static bool WriteChanges()
    {
      try
      {
        var updatedStorage = JsonSerializer.Serialize<AndroidDirectory>(AndroiDirectorySingle.readDirectory);
        using var streamWriter = new StreamWriter(Constants.StoragePath, false);
        streamWriter.Write(updatedStorage);
      }
      catch
      {
        return false;
      }
      return true;
    }
  }
}
