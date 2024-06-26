    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Phone.Connector.Faker.Models.View;
    using Phone.Connector.Faker.Models.ViewModel;
    using Phone.Connector.Faker.Utils;

    namespace Phone.Connector.Faker.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController : Controller
    {
        [HttpPost("ListDirectory")]
        public IActionResult ListDirectory([FromBody] IdDirectoryRequest request)
        {
            DirectoryService directoryService;
            try
            {
                directoryService = new DirectoryService(Constants.StoragePath);
            }
            catch
            {
                return NotFound();
            }

            if (request.path == "/")
            {
                return Ok(new DirectoryElement("/", "directory", 0, directoryService.readDirectory.Directories));
            }

            var foundElement = directoryService.SearchSubDirectory(directoryService.readDirectory.Directories, request.path);
            if (foundElement == null)
            {
                return NotFound();
            }

            return Ok(foundElement);
        }

        [HttpPost("AddDirectory")]
        public IActionResult AddDirectory([FromBody] IdDirectoryRequest request)
        {
            DirectoryService directoryService;
            try
            {
                directoryService = new DirectoryService(Constants.StoragePath);
            }
            catch
            {
                return NotFound();
            }

            var dir = new DirectoryElement(
                request.path,
                "directory",
                0,
                []
            );
            var success = directoryService.AddDirectory(dir);
            if (!success)
            {
                return StatusCode(500);
            }

            directoryService.WriteChanges();
            return Ok();
        }

        [HttpPost("AddFile")]
        public IActionResult AddFile([FromBody] AddFileRequest request)
        {
            DirectoryService directoryService;
            try
            {
                directoryService = new DirectoryService(Constants.StoragePath);
            }
            catch
            {
                return NotFound();
            }

            var file = new DirectoryElement(request.Path, "file", request.SizeBytes, []);
            var success = directoryService.AddFile(file);
            if (!success)
            {
                return StatusCode(500);
            }

            directoryService.WriteChanges();
            return Ok();
        }

        [HttpDelete("RemoveItem")]
        public IActionResult RemoveItem([FromBody] IdDirectoryRequest request)
        {
            DirectoryService directoryService;
            try
            {
                directoryService = new DirectoryService(Constants.StoragePath);
            }
            catch
            {
                return NotFound();
            }

            var success = directoryService.RemoveItem(request.path);
            if (!success)
            {
                return StatusCode(500);
            }

            directoryService.WriteChanges();
            return Ok();
        }

        [HttpPost("MoveItem")]
        public IActionResult MoveItem([FromBody] MoveItemRequest request)
        {
            DirectoryService directoryService;
            try
            {
                directoryService = new DirectoryService(Constants.StoragePath);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }

            var success = directoryService.MoveItem(request.pathToMoveFrom, request.pathToMoveTo);
            if (!success)
            {
                return StatusCode(500);
            }

            directoryService.WriteChanges();
            return Ok();
        }

        [HttpPost("SearchItem")]
        public IActionResult SearchItem([FromBody] SearchItemRequest request)
        {
            DirectoryService directoryService;
            try
            {
                directoryService = new DirectoryService(Constants.StoragePath);
            }
            catch
            {
                return NotFound();
            }

            var searchResults =
                directoryService.SearchItem(request.searchItem, directoryService.readDirectory.Directories);

            return Ok(searchResults);
        }
    }