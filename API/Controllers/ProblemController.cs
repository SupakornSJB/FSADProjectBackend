using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.Tag;
using FSADProjectBackend.Models;
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
    private readonly IProblemUpvoteDownvoteService _problemUpvoteDownvoteService;
    private readonly ITagService _tagService;
    
    public ProblemController(
        IProblemService problemService,
        IProblemUpvoteDownvoteService problemUpvoteDownvoteService,
        ITagService tagService
        )
    {
        _problemService = problemService;
        _problemUpvoteDownvoteService = problemUpvoteDownvoteService;
        _tagService = tagService;
    }
    
    [HttpGet("all")]
    public IActionResult GetAllProblems([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        return Ok(_problemService.GetProblems(page, pageSize));
    }

    [HttpGet("{id}")]   
    public async Task<ActionResult<Problem>> GetProblem(string id)
    {
        try
        {
            var problem = await _problemService.GetProblemById(id);
            await _problemService.IncrementViewCount(problem);
            return Ok(problem);
        }
        catch (Exception ex) when (ex.Message == "Problem not found")
        {
            return NotFound(ex);
        }
    }

    [HttpGet("user")]
    public async Task<ActionResult<Problem>> GetUserCreatedProblem()
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
        await _tagService.UpdateProblemTags(created.Id.ToString(), problem.Tags);
        return CreatedAtAction(nameof(GetProblem), new { id = created.Id }, created);   
    }

    [HttpPut("{id}")] 
    public async Task<IActionResult> UpdateProblemDetail(string id, [FromBody] CreateProblemViewmodel problem)
    {
        try
        {
            var updated = await _problemService.UpdateProblem(id, problem);
            await _tagService.UpdateProblemTags(updated.Id.ToString(), problem.Tags.ToArray());
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

    [HttpPut("{problemId}/upvote-downvote/{isUpvote}")]
    public async Task<IActionResult> ToggleUpvoteDownvoteProblem(string problemId, bool isUpvote)
    {
        await _problemUpvoteDownvoteService.UpvoteOrDownvoteProblem(problemId, isUpvote);
        return Ok();   
    }
}