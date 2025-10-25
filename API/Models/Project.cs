using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class Project
{
    [BsonId]
    [BsonElement("_id")]
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? RelatedProblemId { get; set; }
    public required string BannerPicture { get; set; }
    public required ICollection<ProjectProgressUpdate> ProgressUpdates { get; set; }
    public required Contact Contact { get; set; }
}