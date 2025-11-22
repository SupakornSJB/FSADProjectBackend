using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Comment;
using MongoDB.Bson;

namespace FSADProjectBackend.Services.Problem;

public class ProblemCommentService: IProblemCommentService
{
    private readonly IProblemService _problemService;
    private readonly MongoDbContext _mongoDbContext;
    private readonly IUserInfoService _userInfoService;
    private readonly IProblemCommentUpvoteDownvoteService _problemCommentUpvoteDownvoteService;
    
    public ProblemCommentService(
        IProblemService problemService, 
        MongoDbContext mongoDbContext, 
        IUserInfoService userInfoService,
        IProblemCommentUpvoteDownvoteService problemCommentUpvoteDownvoteService
    )
    {
        _problemService = problemService;
        _mongoDbContext = mongoDbContext;
        _userInfoService = userInfoService;
        _problemCommentUpvoteDownvoteService = problemCommentUpvoteDownvoteService;
    }
    
    public async Task<Comment> GetCommentById(string problemId, string commentId)
    {
        var problem = await _problemService.GetProblemById(problemId);
        var id = new ObjectId(commentId);
        return problem.Comments.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Comment not found");
    }
    
    public async Task<string> CreateComment(string problemId, CreateCommentViewmodel comment)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var problem = await _problemService.GetProblemById(problemId);
        var newComment = comment.ConvertToComment(userInfo);
        problem.Comments.Add(newComment);
        await _mongoDbContext.SaveChangesAsync();
        return newComment.Id.ToString();
    }

    public async Task<Comment[]> GetOrderedCommentsByProblemId(string problemId, int? page = null, int? pageSize = null)
    {
        if ((page == null && pageSize != null) || (page != null && pageSize == null))
        {
            throw new NotSupportedException("Please provide both page and page size or neither");
        }
        
        var problem = await _problemService.GetProblemById(problemId);
        var differenceMapping = _problemCommentUpvoteDownvoteService.GetUpvoteDownvoteDifferenceOfComments(
            problemId, problem.Comments.Select(x => x.Id.ToString()).ToArray());
        var orderedComments = problem.Comments
            .OrderByDescending(x => differenceMapping.GetValueOrDefault(x.Id.ToString(), 0));

        if (page == null || pageSize == null)
        {
            return orderedComments.ToArray();
        }
        
        var skip = (page.Value - 1) * pageSize.Value;
        return orderedComments    
            .Skip(skip)
            .Take(pageSize.Value)
            .ToArray();
    }
    
    public async Task<string> ReplyToComment(string problemId, string parentCommentId, CreateCommentViewmodel comment)
    {
        var parentComment = await GetCommentById(problemId, parentCommentId);
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        if (parentComment == null) throw new Exception("Cannot create reply to comment");
        
        var newComment = comment.ConvertToComment(userInfo);
        parentComment.ChildComments.Add(newComment);
        await _mongoDbContext.SaveChangesAsync();
        return newComment.Id.ToString();
    }
    

    public void DeleteComment(string problemId, string commentId)
    {
    }
}