using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class Project
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }
    [MaxLength(2048)]
    public required string Description { get; set; }
    [MaxLength(256)]
    public string? RelatedProblemId { get; set; }
    [MaxLength(256)]
    public string? RelatedSolutionId { get; set; }
    [MaxLength(256)]
    public required string ProblemSolverId { get; set; }
    [MaxLength(256)] 
    public required string BannerPicture { get; set; }
    public ICollection<ProjectProgressUpdate> ProgressUpdates { get; set; }
}