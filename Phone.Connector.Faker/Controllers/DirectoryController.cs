using Microsoft.AspNetCore.Mvc;
using Phone.Connector.Faker.Models.View;
using Phone.Connector.Faker.Models.ViewModel;
using Phone.Connector.Faker.Utils;

namespace Phone.Connector.Faker.Controllers;

public class DirectoryController : Controller
{
    [HttpPost("/ListDirectory")]
    public IActionResult ListDirectory([FromBody] ListDirectoryRequest request )
    {
        StorageReader storageReader;
        try
        {
            storageReader = new StorageReader("Storage.json");
        }
        catch
        {
            return NotFound();
        }
        
        if (request.path == "/")
        {
            return Ok(storageReader.readDirectory);
        }
        var foundElement = storageReader.SearchSubDirectory(storageReader.readDirectory.Directories, request.path);
        if (foundElement == null)
        {
            return NotFound();
        }
        return Ok(foundElement);
    }

    [HttpPost("/MoveItem")]
    public IActionResult MoveItem([FromBody] MoveItemRequest request)
    {
        StorageReader storageReader;
        try
        {
            storageReader = new StorageReader("Storage.json");
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
        var success = storageReader.MoveItem(request.pathToMoveFrom, request.pathToMoveTo);
        if (success)
        {
            storageReader.WriteChanges();
            return Ok();
        }

        return StatusCode(500);

        
    }
}