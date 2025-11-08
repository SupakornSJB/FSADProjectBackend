using FSADProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProblemCommentController: ControllerBase
{
    [HttpPost("{problemId}", Name = "CreateComment")]
    public IActionResult CreateComment(string problemId, Comment comment) 
    {
        return Ok();
    }

    [HttpPost("{problemId}/{parentCommentId}", Name = "ReplyToComment")]
    public IActionResult ReplyToComment(string problemId, string parentCommentId, [FromBody] Comment comment)
    {
        return Ok();
    }

    [HttpPost("{problemId}/{commentId}/{isUpvote}", Name = "UpvoteOrDownvoteComment")]
    public IActionResult UpvoteOrDownvoteComment(string problemId, string commentId, bool isUpvote)
    {
        return Ok();
    }

    [HttpGet("{problemId}/comments/votes", Name = "GetUpvoteOrDownvoteListOfComments")]
    public IActionResult GetUpvoteOrDownvoteListOfComments(string problemId)
    {
        return Ok();
    }
}
