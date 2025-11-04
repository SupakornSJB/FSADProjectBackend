using FSADProjectBackend.Viewmodels.Problem;

namespace FSADProjectBackend.Interfaces.Problem;

public interface IProblemService
{
    public IEnumerable<Models.Problem> GetProblems();
    public Task<Models.Problem?> GetProblemById(string id);
    public Task<List<Models.Problem>> GetUsersProblems();
    public Task<Models.Problem> CreateProblem(CreateProblemViewmodel problem);
    public Task<Models.Problem> UpdateProblem(string problemId, CreateProblemViewmodel problem);
    public Task DeleteProblem(string problemId);
}