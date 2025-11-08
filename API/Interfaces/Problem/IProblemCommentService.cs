using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Comment;

namespace FSADProjectBackend.Interfaces.Problem;

public interface IProblemCommentService
{
    public Task<Comment> GetCommentById(string problemId, string commentId);

    public Task CreateComment(string problemId, CreateCommentViewmodel comment);

    public Task<Comment[]>
        GetOrderedCommentsByProblemId(string problemId, int? page = null, int? pageSize = null);

    public Task ReplyToComment(string problemId, string parentCommentId, Comment comment);

    public void DeleteComment(string problemId, string commentId);
}