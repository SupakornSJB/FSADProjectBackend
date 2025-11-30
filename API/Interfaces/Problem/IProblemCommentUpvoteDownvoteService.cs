namespace FSADProjectBackend.Interfaces.Problem;

public interface IProblemCommentUpvoteDownvoteService
{
    public Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListOfCommentsMadeByUser(string problemId);

    public Task<Dictionary<string, int>> GetUpvoteOrDownvoteNumberOfComments(string problemId, bool isUpvote);
    
    public Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListOfCommentsMadeByUser(string problemId, string[] comments);

    public Dictionary<string, int> GetUpvoteOrDownvoteNumberOfComments(string problemId, string[] commentIds, bool isUpvote);

    public Dictionary<string, int> GetUpvoteDownvoteDifferenceOfComments(string problemId, string[] commentIds);

    public Task UpvoteOrDownvoteComment(string problemId, string commentId, bool isUpvote);
}