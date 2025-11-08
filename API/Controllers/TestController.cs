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
        return Ok("Hello, this is protected API!");
    }

    [HttpGet("v2")]    
    public IActionResult Get2()
    {
        return Ok("Hello, this is unprotected API!");
    }
}