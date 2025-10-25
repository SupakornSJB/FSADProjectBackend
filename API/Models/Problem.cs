using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class Problem
{
    [BsonId]
    [BsonElement("_id")]
    public required ObjectId Id { get; set; } 
    
    [MaxLength(256)]
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
    
    public required University ParentUniversity { get; set; }
    public required User CreatedBy { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }   
    
    public required ICollection<Comment> Comments { get; set; }
    public required ICollection<ProblemSolution> Solutions { get; set; }
    public required ICollection<Attachment> Attachments { get; set; }
}