using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.ProblemSolver;
using FSADProjectBackend.Interfaces.Project;

namespace FSADProjectBackend.Services.Project;

public class UserProjectService: IUserProjectService
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly IProblemSolverMemberService _problemSolverMemberService;
    
    public UserProjectService(
        MongoDbContext mongoDbContext, 
        IProblemSolverMemberService problemSolverMemberService
    )
    {
        _mongoDbContext = mongoDbContext;
        _problemSolverMemberService = problemSolverMemberService;
    }
    
    public async Task<IEnumerable<Models.Project>> GetProjectsOfUser(int? page = null, int? pageSize = null)
    {
        var usePagination = WillUsePagination(page, pageSize);
        var groupIds = (await _problemSolverMemberService.GetAllProblemSolverGroupOfUser()).Select(x => x.Id);
        if (usePagination)
        {
            return _mongoDbContext.Projects
                .Where(x => groupIds.Contains(x.ProblemSolverId))
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }
        
        return _mongoDbContext.Projects.Where(x => groupIds.Contains(x.ProblemSolverId));
    }

    public IEnumerable<Models.Project> GetProjectsOfUser(string userSubject, int? page = null, int? pageSize = null)
    {
        var usePagination = WillUsePagination(page, pageSize);
        var groupIds = (_problemSolverMemberService.GetAllProblemSolverGroupOfUser(userSubject)).Select(x => x.Id);
        if (usePagination)
        {
            return _mongoDbContext.Projects.Where(x => groupIds.Contains(x.ProblemSolverId))
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }
        
        return _mongoDbContext.Projects.Where(x => groupIds.Contains(x.ProblemSolverId));
    }

    public IEnumerable<Models.Project> GetProjectsOfProblemSolvers(string problemSolverId, int? page = null, int? pageSize = null)
    {
        var usePagination = WillUsePagination(page, pageSize);
        if (usePagination)
        {
            return _mongoDbContext.Projects.Where(x => x.ProblemSolverId == problemSolverId)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }
        
        return _mongoDbContext.Projects.Where(x => x.ProblemSolverId == problemSolverId);
    }

    private bool WillUsePagination(int? page, int? pageSize)
    {
        if ((page != null && pageSize == null) || (page == null && pageSize != null))
        {
            throw new Exception("Please provide both page and page size");
        }

        return !(pageSize == null || page == null);
    }
}