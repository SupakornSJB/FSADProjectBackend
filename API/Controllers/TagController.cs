using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagController: ControllerBase
{
    public TagController() { }
    
    [HttpGet("all")]
    public Task<IActionResult> GetTags()
    {
        throw new NotImplementedException();
    }
}