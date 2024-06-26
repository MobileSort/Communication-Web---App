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
    public IActionResult GetTagById(IdTagRequest request)
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

        return Ok(tagService.GetTagById(request.IdTypeTag));
    }

    [HttpPost("AddTag")]
    public IActionResult AddTag(AddTagRequest request)
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
    public IActionResult GetTypeTagById(IdTagRequest request)
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
    public IActionResult AddTypeTag(AddTypeTagRequest request)
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
}