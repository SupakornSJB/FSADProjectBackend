using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Solution;

namespace FSADProjectBackend.Interfaces.Solution;

public interface ISolutionService
{
    public Task CreateSolution(string problemId, string content);
    public Task<IEnumerable<ProblemSolution>> GetSolutionOfUser();
    public Task<GetProblemAndSolutionViewmodel?> GetProblemAndSolutionById(string problemId, string solutionId);
}