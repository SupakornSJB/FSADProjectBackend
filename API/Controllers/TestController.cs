using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController: ControllerBase
{
    [HttpGet]    
    public IActionResult Get()
    {
        var o = new { a = 1, b = 2 };
        return Ok(o);
    }
}