using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController: ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProjects([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewProject()
    {
        throw new NotImplementedException();
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserInvolvedProject([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        throw new NotImplementedException();
    }
}