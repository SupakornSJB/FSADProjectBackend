using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FSADProjectBackend.Models;

public class University
{
   [BsonId]
   [BsonElement("_id")]
   [BsonRepresentation(BsonType.ObjectId)]
   public required string Id { get; set;} 
   
   [MaxLength(256)]
   public required string Name { get; set;}
   
   [MaxLength(2048)]
   public required string Description { get; set;}
}