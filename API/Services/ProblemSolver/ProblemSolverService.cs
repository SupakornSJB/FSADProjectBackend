using FSADProjectBackend.Contexts;
using FSADProjectBackend.Enums.ProblemSolver;
using FSADProjectBackend.Interfaces.ProblemSolver;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Viewmodels.User;
using Microsoft.EntityFrameworkCore;

namespace FSADProjectBackend.Services.ProblemSolver;

public class ProblemSolverService : IProblemSolverService
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly PgDbContext _pgDbContext;
    private readonly IProblemSolverMemberService _problemSolverMemberService;
    private readonly IUserInfoService _userInfoService;

    public ProblemSolverService(
        PgDbContext pgDbContext,
        MongoDbContext mongoDbContext,
        IProblemSolverMemberService problemSolverMemberService,
        IUserInfoService userInfoService
    )
    {
        _mongoDbContext = mongoDbContext;
        _pgDbContext = pgDbContext;
        _problemSolverMemberService = problemSolverMemberService;
        _userInfoService = userInfoService;
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

    public async Task<Models.ProblemSolver?> DeleteProblemSolver(string id)
    {
        var problemSolver = GetProblemSolverById(id);
        if (problemSolver == null)
        {
            return null;
        }

        await CheckUserPermissions(problemSolver.Id);
        _pgDbContext.ProblemSolvers.Remove(problemSolver);
        await _pgDbContext.SaveChangesAsync();
        return problemSolver;
    }

    public async Task<Models.ProblemSolver> UpdateProblemSolver(string id, Models.ProblemSolver problemSolver)
    {
        var targetProblemSolver = GetProblemSolverById(id);
        if (targetProblemSolver == null)
        {
            throw new Exception("Problem Solver not found");
        }
        
        await CheckUserPermissions(targetProblemSolver.Id);
        targetProblemSolver.Name = problemSolver.Name;
        targetProblemSolver.Description = problemSolver.Description;
        targetProblemSolver.ProfilePic = problemSolver.ProfilePic;
        await _pgDbContext.SaveChangesAsync();
        
        return targetProblemSolver;
    }
    
    private async Task CheckUserPermissions(string problemSolverId)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var allUsers = await _problemSolverMemberService.GetAllMemberOfProblemSolverGroup(userInfo.Subject);
        var currentUser = allUsers.FirstOrDefault(x => x.Subject == userInfo.Subject);

        if (currentUser == null)
        {
            throw new Exception("Do not permission");
        }

        var userPermission = _problemSolverMemberService.GetUserRoleInProblemSolverGroup(problemSolverId, currentUser.Subject);
        if (userPermission == null || userPermission != ProblemSolverRole.Admin || userPermission != ProblemSolverRole.Owner)
        {
            throw new Exception("Do not permission");
        }
    }
}