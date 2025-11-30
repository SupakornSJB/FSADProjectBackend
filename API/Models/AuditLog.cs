using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class AuditLog
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserSubject { get; set; }
    public string? Email { get; set; }
    public string Method { get; set; } = string.Empty;
    [MaxLength(512)]
    public string Path { get; set; } = string.Empty;
    public string? QueryParams { get; set; }
    public int StatusCode { get; set; }
    [MaxLength(100)]
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public int? DurationMs { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}