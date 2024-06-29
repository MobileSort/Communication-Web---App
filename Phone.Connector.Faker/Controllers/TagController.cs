using Microsoft.AspNetCore.Mvc;
using Phone.Connector.Faker.Models.View;
using Phone.Connector.Faker.Models.ViewModel;
using Phone.Connector.Faker.Utils;

namespace Phone.Connector.Faker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : Controller
{
    [HttpPost("ListTags")]
    public IActionResult ListTags()
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        return Ok(tagService.ListTags());
    }

    [HttpPost("GetTagById")]
    public IActionResult GetTagById([FromBody] IdTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        return Ok(tagService.GetTagById(request.IdTag));
    }

    [HttpPost("AddTag")]
    public IActionResult AddTag([FromBody] AddTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        var addedId = tagService.AddTag(request.Name, request.Color, request.TypeTag, request.ValueTag);
        if (addedId == null)
        {
            return NotFound();
        }

        tagService.WriteChanges();
        return Ok(addedId);
    }

    [HttpDelete("RemoveTag")]
    public IActionResult RemoveTag([FromBody] IdTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        var success = tagService.RemoveTag(request.IdTag);
        if (!success)
        {
            return StatusCode(500);
        }

        tagService.WriteChanges();
        return Ok();
    }

    [HttpPost("ListTypeTags")]
    public IActionResult ListTypeTags()
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        return Ok(tagService.ListTypeTags());
    }

    [HttpPost("GetTypeTagById")]
    public IActionResult GetTypeTagById([FromBody] IdTypeTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        return Ok(tagService.GetTypeTagById(request.IdTypeTag));
    }

    [HttpPost("AddTypeTag")]
    public IActionResult AddTypeTag([FromBody] AddTypeTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        var addedId = tagService.AddTypeTag(request.Description);
        if (addedId == null)
        {
            return StatusCode(500);
        }

        tagService.WriteChanges();
        return Ok(addedId);
    }

    [HttpDelete("RemoveTypeTag")]
    public IActionResult RemoveTypeTag([FromBody] IdTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService();
        }
        catch
        {
            return NotFound();
        }

        var success = tagService.RemoveTypeTag(request.IdTag);
        if (!success)
        {
            return StatusCode(500);
        }

        tagService.WriteChanges();
        return Ok();
    }
}