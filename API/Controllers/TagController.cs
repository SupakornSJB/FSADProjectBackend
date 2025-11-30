using FSADProjectBackend.Interfaces.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagController: ControllerBase
{
    private readonly ITagService _tagService; 
    
    public TagController(ITagService tagService)
    {
        _tagService = tagService; 
    }
    
    [HttpGet("all")]
    public IActionResult GetTags([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        return new JsonResult(_tagService.GetTags(page, pageSize));
    }
}