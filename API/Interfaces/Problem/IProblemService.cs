using FSADProjectBackend.Viewmodels.Problem;

namespace FSADProjectBackend.Interfaces.Problem;

public interface IProblemService
{
    public IEnumerable<Models.Problem> GetProblems();
    public IEnumerable<Models.Problem> FilterByPageAndPageSizes(IEnumerable<Models.Problem> problems, int? page,
        int? pageSize);
    public IEnumerable<Models.Problem> FilterByKeywords(IEnumerable<Models.Problem> problems, string keywordString);
    public IEnumerable<Models.Problem> FilterByTags(IEnumerable<Models.Problem> problems, string[] tagNames);
    public Task<Models.Problem> GetProblemById(string id);
    public Task<IEnumerable<Models.Problem>> GetProblemsByIds(string[] ids);
    public Task<List<Models.Problem>> GetUsersProblems();
    public Task<Models.Problem> CreateProblem(CreateProblemViewmodel problem);
    public Task<Models.Problem> UpdateProblem(string problemId, CreateProblemViewmodel problem);
    public Task DeleteProblem(string problemId);
    public Task IncrementViewCount(Models.Problem problem);
}