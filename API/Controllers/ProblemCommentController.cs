using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Viewmodels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProblemCommentController: ControllerBase
{
    private readonly IProblemCommentService _problemCommentService; 
    private readonly IProblemCommentUpvoteDownvoteService _problemCommentUpvoteDownvoteService; 
    
    public ProblemCommentController(
        IProblemCommentService problemCommentService,
        IProblemCommentUpvoteDownvoteService problemCommentUpvoteDownvoteService)
    {
        _problemCommentService = problemCommentService; 
        _problemCommentUpvoteDownvoteService = problemCommentUpvoteDownvoteService;
    }
    
    [HttpPost("{problemId}", Name = "CreateComment")]
    public async Task<IActionResult> CreateComment(string problemId, [FromBody] CreateCommentViewmodel comment) 
    {
        await _problemCommentService.CreateComment(problemId, comment);
        return Ok();
    }

    [HttpPost("{problemId}/{parentCommentId}", Name = "ReplyToComment")]
    public async Task<IActionResult> ReplyToComment(string problemId, string parentCommentId, [FromBody] CreateCommentViewmodel comment)
    {
        await _problemCommentService.ReplyToComment(problemId, parentCommentId, comment);
        return Ok();
    }

    [HttpPut("{problemId}/{commentId}/{isUpvote}", Name = "UpvoteOrDownvoteComment")]
    public async Task<IActionResult> UpvoteOrDownvoteComment(string problemId, string commentId, bool isUpvote)
    {
        await _problemCommentUpvoteDownvoteService.UpvoteOrDownvoteComment(problemId, commentId, isUpvote);
        return Ok();
    }

    [HttpGet("{problemId}/comments/votes", Name = "GetUpvoteOrDownvoteListOfComments")]
    public async Task<IActionResult> GetUpvoteOrDownvoteListOfComments(string problemId)
    {
        return Ok(new
        {
            Upvote = await _problemCommentUpvoteDownvoteService.GetUpvoteOrDownvoteNumberOfComments(problemId, true),
            Downvote = await _problemCommentUpvoteDownvoteService.GetUpvoteOrDownvoteNumberOfComments(problemId, false),
        });
    }
}