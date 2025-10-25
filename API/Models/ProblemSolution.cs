using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class ProblemSolution
{
   [MaxLength(2048)]
   public required string Content { get; set; }
   public required User CreatedBy { get; set; }
   public required DateTime CreatedAt { get; set; }
   public required DateTime UpdatedAt { get; set; }
   
   public required ICollection<Comment> Comments { get; set; }
}