using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class UserProblemCommentVoteMapping
{
    [MaxLength(256)]
    public required string UserSubject { get; set; }
    [MaxLength(256)]
    public required string ProblemId { get; set; }   
    [MaxLength(256)]
    public required string CommentId { get; set; }
    public required bool IsUpvote { get; set; }
}