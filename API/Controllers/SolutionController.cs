using FSADProjectBackend.Interfaces.Solution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

public class UpdateSolutionDto
{
    public string Content { get; set; } 
    public string Status { get; set; }
}

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
    public async Task<IActionResult> CreateSolution(string problemId, [FromBody] UpdateSolutionDto content)
    {
        await _solutionService.CreateSolution(problemId, content.Content, content.Status);
        return Ok();
    }

    [HttpPut("{problemId}/{solutionId}")]
    public async Task<IActionResult> UpdateSolution(string problemId, string solutionId, [FromBody] UpdateSolutionDto dto)
    {
        await _solutionService.UpdateSolution(problemId, solutionId, dto.Content, dto.Status);
        return Ok();
    }
    
    [HttpDelete("{problemId}/{solutionId}")]
    public async Task<IActionResult> DeleteSolution(string problemId, string solutionId)
    {
        await _solutionService.DeleteSolution(problemId, solutionId);
        return Ok();
    }
}