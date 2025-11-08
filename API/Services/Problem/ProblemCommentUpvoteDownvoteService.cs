using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Models;

namespace FSADProjectBackend.Services.Problem;

public class ProblemCommentUpvoteDownvoteService: IProblemCommentUpvoteDownvoteService
{
    private readonly IProblemService _problemService;
    private readonly IUserInfoService _userInfoService;
    private readonly PgDbContext _pgDbContext;
    private readonly IProblemCommentService _problemCommentService;
    
    public ProblemCommentUpvoteDownvoteService (
        PgDbContext pgDbContext, 
        IUserInfoService userInfoService, 
        IProblemService problemService, 
        IProblemCommentService problemCommentService)
    {
        _problemService = problemService;
        _userInfoService = userInfoService;
        _pgDbContext = pgDbContext;
        _problemCommentService = problemCommentService;
    }
    
    public async Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListOfAllCommentsMadeByUser(string problemId)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        return _pgDbContext.UserCommentVoteMappings
            .Where(x => x.ProblemId == problemId && x.UserSubject == userInfo.Subject)
            .ToDictionary(x => x.CommentId, x => x.IsUpvote); 
    }

    public async Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListOfCommentsMadeByUser(string problemId, string[] commentIds)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        return _pgDbContext.UserCommentVoteMappings
            .Where(x => x.ProblemId == problemId && x.UserSubject == userInfo.Subject && commentIds.Contains(x.CommentId))
            .ToDictionary(x => x.CommentId, x => x.IsUpvote);        
    }

    public async Task<Dictionary<string, int>> GetUpvoteOrDownvoteNumberOfAllCommentsOfProblem(string problemId, bool isUpvote)
    {
        var problem = await _problemService.GetProblemById(problemId);
        var commentIds = problem.Comments.Select(x => x.Id.ToString()).ToArray();
        return GetUpvoteOrDownvoteNumberOfComments(problemId, commentIds, isUpvote);
    }
    
    public Dictionary<string, int> GetUpvoteOrDownvoteNumberOfComments(string problemId, string[] commentIds, bool isUpvote)
    {
        if (commentIds.Length == 0) return new Dictionary<string, int>();
        return _pgDbContext.UserCommentVoteMappings
            .Where(x => x.ProblemId == problemId && x.IsUpvote == isUpvote && commentIds.Contains(x.CommentId))
            .GroupBy(x =>  x.CommentId)
            .Select(x => new { CommentId = x.Key, Count = x.Count() })
            .ToDictionary(x => x.CommentId, x => x.Count);
    }

    public Dictionary<string, int> GetUpvoteDownvoteDifferenceOfComments(string problemId, string[] commentIds)
    {
        var upvotesDict = GetUpvoteOrDownvoteNumberOfComments(problemId, commentIds, true);
        var downvoteDict = GetUpvoteOrDownvoteNumberOfComments(problemId, commentIds, false);
        var allCommentIds = upvotesDict.Keys.Concat(downvoteDict.Keys).Distinct().ToArray();
        return allCommentIds.ToDictionary(x => x, x =>
        {
            upvotesDict.TryGetValue(x, out var upvotes);
            downvoteDict.TryGetValue(x, out var downvotes); 
            return upvotes - downvotes;
        });
    }
    
    public async Task UpvoteOrDownvoteComment(string problemId, string commentId, bool isUpvote)
    {
        var comment = await _problemCommentService.GetCommentById(problemId, commentId);
        var mapping = await _pgDbContext.UserCommentVoteMappings.FindAsync(new
        {
            UserSubject = comment.CreatedBy.Subject,
            ProblemId = problemId,
            CommentId = commentId,
        });

        if (mapping == null)
        {
            _pgDbContext.UserCommentVoteMappings.Add(new UserProblemCommentVoteMapping
            {
                UserSubject = comment.CreatedBy.Subject,
                ProblemId = problemId,
                CommentId = commentId,
                IsUpvote = isUpvote
            });
        }
        else
        {
            if (isUpvote == mapping.IsUpvote)
            {
                _pgDbContext.UserCommentVoteMappings.Remove(mapping);
            }
            else
            {
                mapping.IsUpvote = isUpvote;
            }
        }
        
        await _pgDbContext.SaveChangesAsync();
    }
}