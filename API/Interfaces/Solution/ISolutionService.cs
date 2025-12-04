using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Solution;

namespace FSADProjectBackend.Interfaces.Solution;

public interface ISolutionService
{
    public Task CreateSolution(string problemId, string content, string status);
    public Task<IEnumerable<ProblemSolution>> GetSolutionOfUser();
    public Task<GetProblemAndSolutionViewmodel?> GetProblemAndSolutionById(string problemId, string solutionId);
    public Task UpdateSolution(string problemId, string solutionId, string content, string status);
    public Task DeleteSolution(string problemId, string solutionId);
}