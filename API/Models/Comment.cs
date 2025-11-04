using System.ComponentModel.DataAnnotations;
using FSADProjectBackend.Viewmodels.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class Comment
{
    [BsonId]
    [BsonElement("_id")]
    public required ObjectId Id { get; set; }
    
    [MaxLength(2048)]
    public required string Content { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required UserClaimsViewmodel CreatedBy { get; set; }
}