using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Viewmodels.Problem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProblemController: ControllerBase
{
    private readonly IProblemService _problemService;
    
    public ProblemController(IProblemService problemService)
    {
        _problemService = problemService;
    }
    
    [HttpGet("all")]
    public IActionResult GetAllProblems()
    {
        return Ok(_problemService.GetProblems());
    }

    [HttpGet("{id}")]   
    public async Task<IActionResult> GetProblem(string id)
    {
        var problem = await _problemService.GetProblemById(id);
        if (problem == null)
        {
            return NotFound();   
        }
        
        return Ok(problem);   
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserCreatedProblem()
    {
        var problems = await _problemService.GetUsersProblems();
        if (problems.Count == 0)
        {
            return NotFound();   
        }
        
        return Ok(problems);  
    }

    [HttpPost]
    public async Task<IActionResult> CreateProblem(CreateProblemViewmodel problem)
    {
        var created = await _problemService.CreateProblem(problem);
        return CreatedAtAction(nameof(GetProblem), new { id = created.Id }, created);   
    }

    [HttpPut("{id}")] 
    public async Task<IActionResult> UpdateProblemDetail(string id, [FromBody] CreateProblemViewmodel problem)
    {
        try
        {
            var updated = await _problemService.UpdateProblem(id, problem);
            return Ok(updated);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message); 
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProblem(string id)
    {
        try
        {
            await _problemService.DeleteProblem(id);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);         
        }
    }
}