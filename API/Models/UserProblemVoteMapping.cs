using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class UserProblemVoteMapping
{
    [MaxLength(256)]
    public required string UserSubject { get; set; }
    [MaxLength(256)]
    public required string ProblemId { get; set; }
    public required bool IsUpvote { get; set; }   
}