using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class Tag
{
    [MaxLength(256)]
    public required string Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }   
    public required ICollection<ProblemTagMapping> ProblemTagMappings { get; set; }
}