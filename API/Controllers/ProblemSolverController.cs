using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProblemSolverController: ControllerBase
{
    public ProblemSolverController()
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProblemSolvers([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProblemSolverById(string id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("user/{userId}")] 
    public async Task<IActionResult> GetProblemSolverGroupsOfUser([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProblemSolverGroup()
    {
        throw new NotImplementedException();
    }

    [HttpPut("join/{groupId}")]
    public async Task<IActionResult> JoinProblemSolverGroup(string groupId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("invite/{groupId}")]
    public async Task<IActionResult> InviteUsersToProblemSolverGroup(string groupId, [FromBody] string[] userSubjects)
    {
        throw new NotImplementedException();
    }
}