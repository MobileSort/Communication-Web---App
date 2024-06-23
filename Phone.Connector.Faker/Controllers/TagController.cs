using Microsoft.AspNetCore.Mvc;
using Phone.Connector.Faker.Models.View;
using Phone.Connector.Faker.Models.ViewModel;
using Phone.Connector.Faker.Utils;

namespace Phone.Connector.Faker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController: Controller
{
    [HttpPost("ListTags")]
    public IActionResult ListTags()
    {
        TagService tagService;
        try
        {
            tagService = new TagService(Constants.InternalConfigPath);
        }
        catch
        {
            return NotFound();
        }
        return Ok(tagService.ListTags());
    }
    [HttpPost("GetTagByID")]
    public IActionResult GetTagByID(IdTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService(Constants.InternalConfigPath);
        }
        catch
        {
            return NotFound();
        }
        return Ok(tagService.GetTagByID(request.IdTag));
    }
    [HttpPost("ListTypeTags")]
    public IActionResult ListTypeTags()
    {
        TagService tagService;
        try
        {
            tagService = new TagService(Constants.InternalConfigPath);
        }
        catch
        {
            return NotFound();
        }
        return Ok(tagService.ListTypeTags());
    }
    [HttpPost("AddTag")]
    public IActionResult AddTag(AddTagRequest request)
    {
        TagService tagService;
        try
        {
            tagService = new TagService(Constants.InternalConfigPath);
        }
        catch
        {
            return NotFound();
        }

        var addedId = tagService.AddTag(request.Name, request.Color, request.TypeTag,request.ValueTag);
        if (addedId == null)
        {
            return NotFound();
        }

        tagService.WriteChanges();
        return Ok();
    }
}