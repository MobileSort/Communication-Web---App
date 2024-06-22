using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Phone.Connector.Faker.Models.View;
using Phone.Connector.Faker.Models.ViewModel;
using Phone.Connector.Faker.Utils;

namespace Phone.Connector.Faker.Controllers;

public class DirectoryController : Controller
{
    private const string StoragePath = "Storage.json";
    [HttpPost("/ListDirectory")]
    public IActionResult ListDirectory([FromBody] IdDirectoryRequest request )
    {
        StorageReader storageReader;
        try
        {
            storageReader = new StorageReader(StoragePath);
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

    [HttpDelete("RemoveItem")]
    public IActionResult RemoveItem([FromBody] IdDirectoryRequest request)
    {
        StorageReader storageReader;
        try
        {
            storageReader = new StorageReader(StoragePath);
        }
        catch
        {
            return NotFound();
        }

        var success = storageReader.RemoveItem(request.path);
        if (!success)
        {
           return StatusCode(500);
        }
        storageReader.WriteChanges();
        return Ok();
    }

    [HttpPost("/MoveItem")]
    public IActionResult MoveItem([FromBody] MoveItemRequest request)
    {
        StorageReader storageReader;
        try
        {
            storageReader = new StorageReader(StoragePath);
        }
        catch (Exception e)
        {
            return NotFound(e);
        }
        var success = storageReader.MoveItem(request.pathToMoveFrom, request.pathToMoveTo);
        if (!success)
        {
            return StatusCode(500);
        }
        storageReader.WriteChanges();
        return Ok();
    }
}