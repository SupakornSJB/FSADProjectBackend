using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class ProblemSolver
{
    [MaxLength(256)]
    public string Id { get; set; }    
    [MaxLength(256)]
    public required string Name { get; set; }
    [MaxLength(256)]
    public required string ProfilePic { get; set; }
    public ICollection<UserProblemSolverMapping> MembersMapping { get; set; }
    [MaxLength(2048)]
    public required string Description { get; set; }
    public Contact contact { get; set;  }
}