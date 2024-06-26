using System.Text.Json;
using Phone.Connector.Faker.Models.ViewModel;

namespace Phone.Connector.Faker.Utils;

public static class InternalConfigSingle
{
    private static InternalConfig? _internalConfigRead;
    public static InternalConfig internalConfigRead
    {
        get
        {
            if (_internalConfigRead == null)
            {
                using StreamReader r = new(Constants.InternalConfigPath);

                string json = r.ReadToEnd();
                var foundConfig = JsonSerializer.Deserialize<InternalConfig>(json);
                if (foundConfig == null)
                {
                    throw new Exception("Not Found");
                }

                _internalConfigRead = foundConfig;
            }

            return _internalConfigRead;
        }
        set
        {
            _internalConfigRead = value;
        }
    }

    public static bool WriteChanges()
    {
        try
        {
            var updatedStorage = JsonSerializer.Serialize<InternalConfig>(InternalConfigSingle.internalConfigRead);
            using var streamWriter = new StreamWriter(Constants.InternalConfigPath, false);
            streamWriter.Write(updatedStorage);
        }
        catch
        {
            return false;
        }
        return true;
    }
}