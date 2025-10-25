using System.ComponentModel.DataAnnotations;

namespace FSADProjectBackend.Models;

public class Attachment
{
    [MaxLength(256)]
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}