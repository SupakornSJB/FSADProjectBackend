using System.ComponentModel.DataAnnotations;
using FSADProjectBackend.Enums.ProblemSolver;

namespace FSADProjectBackend.Models;

public class UserProblemSolverMapping
{
    [MaxLength(256)]
    public required string UserSubject { get; set; }
    [MaxLength(256)]
    public required string ProblemSolverId { get; set; }   
    public ProblemSolver ProblemSolver { get; set; }   
    public required DateTime InvitedAt { get; set; }
    public DateTime? JoinedAt { get; set; }
    
    [EnumDataType(typeof(ProblemSolverRole))]
    public required ProblemSolverRole Role { get; set; }
}