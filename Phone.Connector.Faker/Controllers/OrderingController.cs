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
        //throw this to the service of ordering
        using StreamReader readerStorage = new(Constants.BackupStoragePath);
        
        string json = readerStorage.ReadToEnd();
        if (json == null)
        {
            throw new Exception("Not Found");
        }
        try
        {
            using var writerStorage = new StreamWriter(Constants.StoragePath, false);
            writerStorage.Write(json);
        }
        catch
        {
            return false;
        }
        return true;
    }

    [HttpPost("")]
    public IActionResult GetOrderings()
    {
        using StreamReader readerStorage = new(Constants.InternalConfigPath);
        
        string json = readerStorage.ReadToEnd();
        if (json == null)
        {
            throw new Exception("Not Found");
        }
        
        return Ok();
    }
}