using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.Solution;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Solution;
using MongoDB.Bson;

namespace FSADProjectBackend.Services.Solution;

public class SolutionService: ISolutionService
{
    private readonly IProblemService _problemService;
    private readonly MongoDbContext _mongoDbContext;
    private readonly IUserInfoService _userInfoService;
    
    public SolutionService(IProblemService problemService, MongoDbContext mongoDbContext, IUserInfoService userInfoService)
    {
        _problemService = problemService;
        _mongoDbContext = mongoDbContext;
        _userInfoService = userInfoService;
    }
    
    public async Task CreateSolution(string problemId, string content, string status)
    {
        var problem = await _mongoDbContext.Problems.FindAsync(problemId);
        if (problem == null)
        {
            throw new Exception("Problem not found");     
        }

        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        problem.Solutions.Add(
            new ProblemSolution
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Content = content,
                Comments = new List<Comment>(),
                CreatedAt = new DateTime(),
                UpdatedAt = new DateTime(),
                CreatedBy = userInfo,
                Status = status
            }
        );

        await _mongoDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProblemSolution>> GetSolutionOfUser()
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var userProblems = await _problemService.GetUsersProblems();

        var allSolutionsByUser = new List<ProblemSolution>();
        
        foreach (var problem in userProblems)
        {
            var solutions = problem.Solutions.Where(x => x.CreatedBy.Subject == userInfo.Subject);
            allSolutionsByUser.AddRange(solutions); 
        }

        return allSolutionsByUser;
    }

    public async Task<GetProblemAndSolutionViewmodel?> GetProblemAndSolutionById(string problemId, string solutionId)
    {
        var problem = await _problemService.GetProblemById(problemId);
        var solution = problem?.Solutions.FirstOrDefault(x => x.Id.ToString() == solutionId);
        if (solution == null || problem == null) return null;

        return new GetProblemAndSolutionViewmodel
        {
            Problem = problem,
            Solution = solution
        };
    }

    public async Task UpdateSolution(string problemId, string solutionId, string content, string status)
    {
        var problem = await _problemService.GetProblemById(problemId);
        var solution = problem?.Solutions.FirstOrDefault(x => x.Id.ToString() == solutionId);
        if (solution == null || problem == null)
        {
            throw new Exception("Problem or solution not found"); 
        }
        
        solution.Content = content;
        solution.Status = status;
        solution.UpdatedAt = DateTime.Now;
        await _mongoDbContext.SaveChangesAsync();
    }

    public async Task DeleteSolution(string problemId, string solutionId)
    {
        var problem = await _problemService.GetProblemById(problemId);
        var solution = problem?.Solutions.FirstOrDefault(x => x.Id.ToString() == solutionId);
        if (solution == null || problem == null)
        {
            throw new Exception("Problem or solution not found"); 
        }
        
        problem.Solutions.Remove(solution);
        await _mongoDbContext.SaveChangesAsync();
    }
}