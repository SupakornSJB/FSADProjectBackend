using FSADProjectBackend.Contexts;
using FSADProjectBackend.Enums.ProblemSolver;
using FSADProjectBackend.Interfaces.ProblemSolver;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.ProblemSolver;
using Microsoft.EntityFrameworkCore;
using Shared.Viewmodels;

namespace FSADProjectBackend.Services.ProblemSolver;

public class ProblemSolverMemberService: IProblemSolverMemberService
{
    private readonly PgDbContext _pgDbContext;
    private readonly IUserInfoService _userInfoService;
    
    public ProblemSolverMemberService(PgDbContext pgDbContext, IUserInfoService userInfoService)
    {
       _pgDbContext = pgDbContext; 
       _userInfoService = userInfoService;
    }
    
    public async Task<IEnumerable<PublicUserViewmodel>> GetAllMemberOfProblemSolverGroup(string id, bool includePendingInvite = true)
    {
        IEnumerable<UserProblemSolverMapping> mapping;
        mapping = includePendingInvite ? 
            _pgDbContext.UserProblemSolverMappings.Where(x => x.ProblemSolverId == id) : 
            _pgDbContext.UserProblemSolverMappings.Where(x => x.ProblemSolverId == id && x.JoinedAt != null);

        return await Task.WhenAll(mapping.Select(async x => 
            await _userInfoService.GetUserInfoAsUserClaimsVm(x.UserSubject)));
    }

    public async Task AddOwnerToProblemSolverGroup(string problemSolverGroupId)
    {
        if (CheckOwnerExists(problemSolverGroupId))
        {
            throw new Exception("Owner already exists"); 
        }
        
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        _pgDbContext.UserProblemSolverMappings.Add(new UserProblemSolverMapping
        {
            UserSubject = userInfo.Subject,
            ProblemSolverId = problemSolverGroupId,
            InvitedAt = new DateTime(),
            JoinedAt = new DateTime(),
            Role = ProblemSolverRole.Owner
        });
        
        await _pgDbContext.SaveChangesAsync();
    }

    public async Task InviteUsersToProblemSolverGroup(string problemSolverGroupId, ProblemSolverRoleMapViewmodel[] userRoleMap)
    {
        await CheckOwnerPermission(problemSolverGroupId);
        var newInviteList = new List<UserProblemSolverMapping>();
        foreach (var map in userRoleMap)
        {
            var existMap = await _pgDbContext.UserProblemSolverMappings.FindAsync(problemSolverGroupId, map.UserSubject);
            if (existMap != null && existMap.JoinedAt != null)
            {
                continue;                
            }

            if (existMap == null)
            {
                newInviteList.Add(new UserProblemSolverMapping
                {
                    UserSubject = map.UserSubject,
                    ProblemSolverId = problemSolverGroupId,
                    InvitedAt = new DateTime(),
                    Role = map.Role
                });
            }
            else
            {
                existMap.Role = map.Role;
            }
        }
        
        _pgDbContext.UserProblemSolverMappings.AddRange(newInviteList);
        await _pgDbContext.SaveChangesAsync();
    }

    public async Task<string> RemoveUsersFromProblemSolverGroup(string problemSolverGroupId, string[] userSubject)
    {
        await CheckOwnerPermission(problemSolverGroupId);
        var removedUsers = _pgDbContext.UserProblemSolverMappings.Where(x => x.ProblemSolverId == problemSolverGroupId && userSubject.Contains(x.UserSubject));
        _pgDbContext.UserProblemSolverMappings.RemoveRange(removedUsers);
        await _pgDbContext.SaveChangesAsync();
        return string.Join(",", userSubject);
    }

    public async Task JoinProblemSolverGroup(string problemSolverGroupId)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var invite = _pgDbContext.UserProblemSolverMappings.Find(problemSolverGroupId, userInfo.Subject); 
        if (invite == null || invite.JoinedAt != null)
        {
            throw new Exception("Cannot join problem solver group. You are not invited or already joined"); 
        }
        
        invite.JoinedAt = new DateTime();
        await _pgDbContext.SaveChangesAsync(); 
    }

    public async Task<IEnumerable<Models.ProblemSolver>> GetAllProblemSolverGroupOfUser()
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var allMapping = _pgDbContext.UserProblemSolverMappings
            .Where(x => x.UserSubject == userInfo.Subject)
            .Include(x => x.ProblemSolver);
        return allMapping.DistinctBy(x => x.ProblemSolverId).Select(x => x.ProblemSolver);
    }

    private bool CheckOwnerExists(string problemSolverGroupId)
    {
        var existingOwner = _pgDbContext.UserProblemSolverMappings
            .Where(x => x.ProblemSolverId == problemSolverGroupId && x.Role == ProblemSolverRole.Owner);
        
        return existingOwner.Any();
    }

    private async Task CheckOwnerPermission(string problemSolverGroupId)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var userRole = GetUserRoleInProblemSolverGroup(problemSolverGroupId, userInfo.Subject);
        if (userRole != ProblemSolverRole.Owner)
        {
            throw new Exception("Cannot invite users to problem solver group. Only owner can invite users");
        }
    }
    
    private ProblemSolverRole? GetUserRoleInProblemSolverGroup(string problemSolverGroupId, string userSubject)
    {
        var userRole = _pgDbContext.UserProblemSolverMappings.Find(problemSolverGroupId, userSubject);
        if (userRole == null)
        {
            return null;
        }
        
        return userRole.Role;
    }
}