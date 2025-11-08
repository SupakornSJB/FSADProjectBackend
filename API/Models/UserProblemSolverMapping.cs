using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class UserProblemSolverMapping
{
    [MaxLength(256)]
    public required string UserSubject { get; set; }
    [MaxLength(256)]
    public required string ProblemSolverId { get; set; }   
    public required ProblemSolver ProblemSolver { get; set; }   
    public required DateTime JoinedAt { get; set; }
}