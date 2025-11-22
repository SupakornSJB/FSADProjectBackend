using FSADProjectBackend.Models;

namespace FSADProjectBackend.Interfaces.Solution;

public interface ISolutionService
{
    public Task CreateSolution(string problemId, string content);
    public Task<IEnumerable<ProblemSolution>> GetSolutionOfUser();
}