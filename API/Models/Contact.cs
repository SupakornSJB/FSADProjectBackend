using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class Contact
{
    [MaxLength(256)]
    public string ProblemSolverId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Twitter { get; set; }
    public string? LinkedIn { get; set; }
}