using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class ProblemSolver
{
    [MaxLength(256)]
    public required string Id { get; set; }    
    [MaxLength(256)]
    public required string Name { get; set; }
    [MaxLength(256)]
    public required string ProfilePic { get; set; }
    public required ICollection<UserProblemSolverMapping> MembersMapping { get; set; }
}