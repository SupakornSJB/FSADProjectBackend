using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.User;
using MongoDB.Bson;

namespace FSADProjectBackend.Viewmodels.Problem;

public class CreateProblemViewmodel
{
    public required string Name { get; set; }
    public required string Content { get; set; }
    
    public ICollection<Attachment> Attachments { get; set; }   
    public ICollection<string> Tags { get; set; }   
}

public static class CreateProblemViewmodelExtension
{
    public static Models.Problem ConvertToProblem(this CreateProblemViewmodel viewmodel, UserClaimsViewmodel claims)
    {
        return new Models.Problem
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = viewmodel.Name,
            Content = viewmodel.Content,
            Attachments = viewmodel.Attachments ?? new List<Attachment>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Solutions = new List<ProblemSolution>(),
            Comments = new List<Models.Comment>(),
            CreatedBy = claims,
            ViewCount = 0,
            Status = "Open" 
        };
    }
} 