using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class Tag
{
    [MaxLength(256)]
    public string Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }   
    public ICollection<ProblemTagMapping> ProblemTagMappings { get; set; }
}