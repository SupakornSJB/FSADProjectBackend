using System.ComponentModel.DataAnnotations;
using FSADProjectBackend.Viewmodels.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class Problem
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; } 
    
    [MaxLength(256)]
    public required string Name { get; set; }
    
    [MaxLength(4096)]
    public required string Content { get; set; }
    
    public required UserClaimsViewmodel CreatedBy { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }   
    
    public ICollection<Comment> Comments { get; set; }
    public ICollection<ProblemSolution> Solutions { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
}