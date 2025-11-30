using FSADProjectBackend.Viewmodels.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class ProjectProgressUpdate
{
    [BsonId]
    [BsonElement("_id")]
    public required ObjectId Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }   
    public required DateTime UpdatedAt { get; set; }   
    public required UserClaimsViewmodel CreatedBy { get; set; }  
    public required ICollection<Attachment> Attachments { get; set; }
    public required ICollection<Comment> Comments { get; set; }   
}