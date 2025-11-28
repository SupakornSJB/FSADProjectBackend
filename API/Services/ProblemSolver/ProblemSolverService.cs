using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.ProblemSolver;
using FSADProjectBackend.Viewmodels.User;
using Microsoft.EntityFrameworkCore;

namespace FSADProjectBackend.Services.ProblemSolver;

public class ProblemSolverService: IProblemSolverService
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly PgDbContext _pgDbContext;
    private readonly IProblemSolverMemberService _problemSolverMemberService;
    
    public ProblemSolverService(
        PgDbContext pgDbContext, 
        MongoDbContext mongoDbContext, 
        IProblemSolverMemberService problemSolverMemberService
         
        )
    {
        _mongoDbContext = mongoDbContext;
        _pgDbContext = pgDbContext;
        _problemSolverMemberService = problemSolverMemberService;
    }
    
    public IEnumerable<Models.ProblemSolver> GetProblemSolvers(int? page = null, int? pageSize = null)
    {
        if ((page == null && pageSize != null) || (page != null && pageSize == null))
        {
            throw new NotSupportedException("Please provide both page and page size or neither");
        }

        if (page == null || pageSize == null)
        {
            return _pgDbContext.ProblemSolvers;
        }

        return _pgDbContext.ProblemSolvers
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value);
    }

    public Models.ProblemSolver? GetProblemSolverById(string id)
    {
        return _pgDbContext.ProblemSolvers
            .Include(x => x.MembersMapping)
            .FirstOrDefault(x => x.Id == id);
    }

    public async Task<Models.ProblemSolver> CreateProblemSolver(Models.ProblemSolver problemSolver)
    {
        var newProblemSolver = new Models.ProblemSolver
        {
            Name = problemSolver.Name,
            ProfilePic = problemSolver.ProfilePic,
            Description = problemSolver.Description
        };
        
        var entity = await _pgDbContext.ProblemSolvers.AddAsync(newProblemSolver);
        await _problemSolverMemberService.AddOwnerToProblemSolverGroup(entity.Entity.Id);
        await _pgDbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public Models.ProblemSolver? DeleteProblemSolver(string id)
    {
        var problemSolver = GetProblemSolverById(id);
        if (problemSolver == null)
        {
            return null;
        }

        _pgDbContext.ProblemSolvers.Remove(problemSolver);
        _pgDbContext.SaveChanges();
        return problemSolver;
    }

    public Task UpdateProblemSolver(string id, Models.ProblemSolver problemSolver)
    {
        throw new NotImplementedException();
    }
}