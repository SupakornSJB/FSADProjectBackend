using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class ProblemTagMapping
{
    [MaxLength(256)]
    public required string TagId { get; set; }
    public Tag Tag { get; set; }
    [MaxLength(256)]
    public required string ProblemId { get; set; }   
}