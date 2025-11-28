using FSADProjectBackend.Viewmodels.User;

namespace FSADProjectBackend.Interfaces.ProblemSolver;

public interface IProblemSolverService
{
    public IEnumerable<Models.ProblemSolver> GetProblemSolvers(int? page = null, int? pageSize = null); 
    public Models.ProblemSolver? GetProblemSolverById(string id);
    public Task<Models.ProblemSolver> CreateProblemSolver(Models.ProblemSolver problemSolver);
    public Models.ProblemSolver? DeleteProblemSolver(string id);
    public Task UpdateProblemSolver(string id, Models.ProblemSolver problemSolver);
}