namespace FSADProjectBackend.Interfaces.Problem;

public interface IProblemUpvoteDownvoteService
{
    public Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListMadeByUser();

    public Task<Dictionary<string, bool>> GetUpvoteOrDownvoteListMadeByUser(string[] problemIds);

    public Dictionary<string, int> GetUpvoteOrDownvoteNumber(bool isUpvote);

    public Dictionary<string, int> GetUpvoteOrDownvoteNumber(string[] problemIds, bool isUpvote);

    public Dictionary<string, int> GetUpvoteDownvoteDifference(string[] problemIds);

    public Task UpvoteOrDownvoteProblem(string problemId, bool isUpvote);
}