using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Models;

namespace FSADProjectBackend.Services.Problem;

public class ProblemUpvoteDownvoteService: IProblemUpvoteDownvoteService
{    
    private readonly IProblemService _problemService;
    private readonly IUserInfoService _userInfoService;
    private readonly PgDbContext _pgDbContext;
    
    public ProblemUpvoteDownvoteService (
        PgDbContext pgDbContext, 
        IUserInfoService userInfoService, 
        IProblemService problemService)
    {
        _problemService = problemService;
        _userInfoService = userInfoService;
        _pgDbContext = pgDbContext;
    }
    
    public async Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListMadeByUser()
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        return _pgDbContext.UserProblemVoteMappings
            .Where(x => x.UserSubject == userInfo.Subject)
            .ToDictionary(x => x.ProblemId, x => x.IsUpvote); 
    }

    public async Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListMadeByUser(string[] problemIds)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        return _pgDbContext.UserProblemVoteMappings
            .Where(x => x.UserSubject == userInfo.Subject &&  problemIds.Contains(x.ProblemId))
            .ToDictionary(x => x.ProblemId, x => x.IsUpvote); 
    }
    
    public Dictionary<string, int> GetUpvoteOrDownvoteNumber(bool isUpvote)
    {
        return _pgDbContext.UserProblemVoteMappings
            .Where(x => x.IsUpvote == isUpvote)
            .GroupBy(x =>  x.ProblemId)
            .Select(x => new { ProblemId = x.Key, Count = x.Count() })
            .ToDictionary(x => x.ProblemId, x => x.Count);
    }
    
    public Dictionary<string, int> GetUpvoteOrDownvoteNumber(string[] problemIds, bool isUpvote)
    {
        if (problemIds.Length == 0) return new Dictionary<string, int>();
        return _pgDbContext.UserProblemVoteMappings
            .Where(x => x.IsUpvote == isUpvote && problemIds.Contains(x.ProblemId))
            .GroupBy(x =>  x.ProblemId)
            .Select(x => new { ProblemId = x.Key, Count = x.Count() })
            .ToDictionary(x => x.ProblemId, x => x.Count);
    }

    public Dictionary<string, int> GetUpvoteDownvoteDifference(string[] problemIds)
    {
        var upvotesDict = GetUpvoteOrDownvoteNumber(problemIds, true);
        var downvoteDict = GetUpvoteOrDownvoteNumber(problemIds, false);
        var allCommentIds = upvotesDict.Keys.Concat(downvoteDict.Keys).Distinct().ToArray();
        return allCommentIds.ToDictionary(x => x, x =>
        {
            upvotesDict.TryGetValue(x, out var upvotes);
            downvoteDict.TryGetValue(x, out var downvotes); 
            return upvotes - downvotes;
        });
    }
    
    public async Task UpvoteOrDownvoteProblem(string problemId, bool isUpvote)
    {
        var comment = await _problemService.GetProblemById(problemId);
        var mapping = await _pgDbContext.UserProblemVoteMappings.FindAsync( comment.CreatedBy.Subject, problemId );

        if (mapping == null)
        {
            _pgDbContext.UserProblemVoteMappings.Add(new UserProblemVoteMapping()
            {
                UserSubject = comment.CreatedBy.Subject,
                ProblemId = problemId,
                IsUpvote = isUpvote
            });
        }
        else
        {
            if (isUpvote == mapping.IsUpvote)
            {
                _pgDbContext.UserProblemVoteMappings.Remove(mapping);
            }
            else
            {
                mapping.IsUpvote = isUpvote;
            }
        }
        
        await _pgDbContext.SaveChangesAsync();
    }
}