using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Comment;

namespace FSADProjectBackend.Interfaces.Problem;

public interface IProblemCommentService
{
    public Task<Comment> GetCommentById(string problemId, string commentId);

    public Task<string> CreateComment(string problemId, CreateCommentViewmodel comment);

    public Task<Comment[]>
        GetOrderedCommentsByProblemId(string problemId, int? page = null, int? pageSize = null);

    public Task<string> ReplyToComment(string problemId, string parentCommentId, CreateCommentViewmodel comment);

    public Task DeleteComment(string problemId, string commentId);
    public Task UpdateComment(string problemId, string commentId, string content);
}