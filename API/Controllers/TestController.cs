using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController: ControllerBase
{
    [Authorize]
    [HttpGet]    
    public IActionResult Get()
    {
        var o = new { a = 1, b = 2 };
        return Ok(o);
    }

    [HttpGet("v2")]    
    public IActionResult Get2()
    {
        var o = new { c = 1, d = 2 };
        return Ok(o);
    }
}