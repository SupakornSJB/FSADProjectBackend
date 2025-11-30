using FSADProjectBackend.Enums.ProblemSolver;
using FSADProjectBackend.Viewmodels.ProblemSolver;
using Shared.Viewmodels;

namespace FSADProjectBackend.Interfaces.ProblemSolver;

public interface IProblemSolverMemberService
{
    public Task<IEnumerable<PublicUserViewmodel>> GetAllMemberOfProblemSolverGroup(string problemSolverId, bool includePendingInvite = true);
    public Task AddOwnerToProblemSolverGroup(string problemSolverGroupId);
    public Task InviteUsersToProblemSolverGroup(string problemSolverGroupId, ProblemSolverRoleMapViewmodel[] userRoleMap);
    public Task<string> RemoveUsersFromProblemSolverGroup(string problemSolverGroupId, string[] userSubject);
    public Task JoinProblemSolverGroup(string problemSolverGroupId);
    public Task<IEnumerable<Models.ProblemSolver>> GetAllProblemSolverGroupOfUser();
    public IEnumerable<Models.ProblemSolver> GetAllProblemSolverGroupOfUser(string userSubject);
    public ProblemSolverRole? GetUserRoleInProblemSolverGroup(string problemSolverGroupId, string userSubject);
    public Task<bool> CheckIfUserBelongInProblemSolverGroup(string problemSolverId);
}