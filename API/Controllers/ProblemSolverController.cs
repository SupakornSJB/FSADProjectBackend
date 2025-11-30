using FSADProjectBackend.Interfaces.ProblemSolver;
using FSADProjectBackend.Viewmodels.ProblemSolver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProblemSolverController: ControllerBase
{
    private readonly IProblemSolverService _problemSolverService;
    private readonly IProblemSolverMemberService _problemSolverMemberService; 
    
    public ProblemSolverController(
        IProblemSolverService problemSolverService,
        IProblemSolverMemberService problemSolverMemberService
    )
    {
        _problemSolverService = problemSolverService;
        _problemSolverMemberService = problemSolverMemberService;
    }
    
    [HttpGet]
    public IActionResult GetProblemSolvers([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        return new JsonResult(_problemSolverService.GetProblemSolvers(page, pageSize));
    }

    [HttpGet("{id}")]
    public IActionResult GetProblemSolverById(string id)
    {
        return new JsonResult(_problemSolverService.GetProblemSolverById(id));
    }

    [HttpGet("user/{userId}")] 
    public async Task<IActionResult> GetProblemSolverGroupsOfUser([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        return new JsonResult(await _problemSolverMemberService.GetAllProblemSolverGroupOfUser());
    }

    [HttpPost]
    public async Task<IActionResult> CreateProblemSolverGroup([FromBody] Models.ProblemSolver problemSolver)
    {
        var newProblemSolver = await _problemSolverService.CreateProblemSolver(problemSolver);
        return new JsonResult(newProblemSolver);
    }

    [HttpPut("join/{groupId}")]
    public async Task<IActionResult> JoinProblemSolverGroup(string groupId)
    {
        await _problemSolverMemberService.JoinProblemSolverGroup(groupId);
        return Ok();
    }

    [HttpPut("invite/{groupId}")]
    public async Task<IActionResult> InviteUsersToProblemSolverGroup(string groupId, [FromBody] ProblemSolverRoleMapViewmodel[] userSubjects)
    {
        await _problemSolverMemberService.InviteUsersToProblemSolverGroup(groupId, userSubjects);
        return Ok();
    }

    [HttpDelete("{groupId}")]
    public async Task<IActionResult> DeleteProblemSolverGroup(string groupId)
    {
        await _problemSolverService.DeleteProblemSolver(groupId);
        return Ok();
    }
}