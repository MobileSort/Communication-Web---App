using Microsoft.AspNetCore.Mvc;
using Phone.Connector.Faker.Utils;

namespace Phone.Connector.Faker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderingController: Controller
{
    [HttpPost("RevertChanges")]
    public bool RevertChages()
    {
        using StreamReader r = new(Constants.BackupStoragePath);
        
        string json = r.ReadToEnd();
        if (json == null)
        {
            throw new Exception("Not Found");
        }
        try
        {
            using var streamWriter = new StreamWriter(Constants.StoragePath, false);
            streamWriter.Write(json);
        }
        catch
        {
            return false;
        }
        return true;
    }
}