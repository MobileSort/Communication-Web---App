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
        StorageReader storageReader = new("Storage.json");
        AndroidDirectory? androidRoot = storageReader.ReadStorage();
        if (androidRoot == null)
        {
            return NotFound();
        }

        var foundElement = storageReader.SearchSubDirectory(androidRoot.Directories, request.path);
        if (foundElement == null)
        {
            return NotFound();
        }
        return Ok(foundElement);
    }
}