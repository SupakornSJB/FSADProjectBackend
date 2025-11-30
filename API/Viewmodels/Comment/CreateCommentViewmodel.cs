using FSADProjectBackend.Viewmodels.User;
using MongoDB.Bson;

namespace FSADProjectBackend.Viewmodels.Comment;

public class CreateCommentViewmodel
{
    public required string Content { get; set; }
}

public static class CreateCommentViewmodelExtension
{
    public static Models.Comment ConvertToComment(this CreateCommentViewmodel viewmodel, UserClaimsViewmodel claims)
    {
        return new Models.Comment
        {
            Id = ObjectId.GenerateNewId(),
            Content = viewmodel.Content,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CreatedBy = claims,
            ChildComments = new List<Models.Comment>()
        };
    }
}