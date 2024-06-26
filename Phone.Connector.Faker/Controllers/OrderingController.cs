using Microsoft.AspNetCore.Mvc;
using Phone.Connector.Faker.Models.View;
using Phone.Connector.Faker.Models.ViewModel;
using Phone.Connector.Faker.Utils;

namespace Phone.Connector.Faker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderingController : Controller
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

    [HttpPost("ListOrderings")]
    public IActionResult ListOrderings()
    {
        OrderingService service = new();
        return Ok(service.ListOrderings());
    }

    [HttpPost("GetOrderingById")]
    public IActionResult GetOrderingById([FromBody] IdOrderingRequest request)
    {
        OrderingService service = new();
        return Ok(service.GetOrderingById(request.IdOrdering));
    }

    [HttpPost("AddOrdering")]
    public IActionResult AddOrdering([FromBody] AddOrderingRequest request)
    {
        OrderingService orderingService;
        try
        {
            orderingService = new OrderingService();
        }
        catch
        {
            return NotFound();
        }

        var addedId = orderingService.AddOrdering(request.Name, request.Tags, request.DirectoryDestination);
        if (addedId == null)
        {
            return NotFound();
        }

        orderingService.WriteChanges();
        return Ok(addedId);
    }

    [HttpDelete("RemoveOrdering")]
    public IActionResult RemoveOrdering([FromBody] IdOrderingRequest request)
    {
        OrderingService orderingService;
        try
        {
            orderingService = new OrderingService();
        }
        catch
        {
            return NotFound();
        }

        var success = orderingService.RemoveOrdering(request.IdOrdering);
        if (!success)
        {
            return StatusCode(500);
        }

        orderingService.WriteChanges();
        return Ok();
    }

    [HttpPost("ExecuteOrdering")]
    public IActionResult ExecuteOrdering()
    {
        return StatusCode(501);
    }
}