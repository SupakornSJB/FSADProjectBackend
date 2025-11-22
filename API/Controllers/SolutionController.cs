using FSADProjectBackend.Interfaces.Solution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SolutionController: ControllerBase
{
    private readonly ISolutionService _solutionService;
    
    public SolutionController(ISolutionService solutionService)
    {
        _solutionService = solutionService;
    }
        
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetProposedSolutionOfUser(string userId)
    {
        var solutions = await _solutionService.GetSolutionOfUser();
        return Ok(solutions);
    }

    [HttpPost("{problemId}")]
    public async Task<IActionResult> CreateSolution(string problemId, string content)
    {
        await _solutionService.CreateSolution(problemId, content);
        return Ok();
    }
}