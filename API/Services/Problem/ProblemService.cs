using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Viewmodels.Problem;
using MongoDB.Bson;

namespace FSADProjectBackend.Services.Problem;

public class ProblemService: IProblemService
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly IUserInfoService _userInfoService;
    
    public ProblemService(MongoDbContext mongoDbContext, IUserInfoService userInfoService)
    {
       _mongoDbContext = mongoDbContext; 
       _userInfoService = userInfoService;
    }
    
    public IEnumerable<Models.Problem> GetProblems()
    {
        return _mongoDbContext.Problems;
    }

    public async Task<Models.Problem?> GetProblemById(string id)
    {
        return await _mongoDbContext.Problems.FindAsync(new ObjectId(id));
    }

    public async Task<List<Models.Problem>> GetUsersProblems()
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        return _mongoDbContext.Problems.Where(p => p.CreatedBy.Subject == userInfo.Subject).ToList();
    }

    public async Task<Models.Problem> CreateProblem(CreateProblemViewmodel problem)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        var addedEntity = await _mongoDbContext.Problems.AddAsync(problem.ConvertToProblem(userInfo));
        await _mongoDbContext.SaveChangesAsync();
        return addedEntity.Entity;
    }

    public async Task<Models.Problem> UpdateProblem(string problemId, CreateProblemViewmodel problem)
    {
        var selectedProblem = await GetProblemById(problemId);
        if (selectedProblem == null)
        {
            throw new Exception("Problem not found");
        }

        await VerifyProblemAccess(selectedProblem);
        
        selectedProblem.Name = problem.Name;
        selectedProblem.Content = problem.Content;
        selectedProblem.Attachments = problem.Attachments;
        
        await _mongoDbContext.SaveChangesAsync();
        return selectedProblem;
    }

    public async Task DeleteProblem(string problemId)
    {
        var selectedProblem = await GetProblemById(problemId);
        if (selectedProblem == null)
        {
            throw new Exception("Problem not found");
        }
        
        await VerifyProblemAccess(selectedProblem);
        
        _mongoDbContext.Problems.Remove(selectedProblem);
        await _mongoDbContext.SaveChangesAsync();
    }

    private async Task VerifyProblemAccess(Models.Problem problem)
    {
        var userInfo = await _userInfoService.GetUserInfoAsUserClaimsVm();
        
        if (string.IsNullOrEmpty(problem.CreatedBy.Subject) ||
            problem.CreatedBy.Subject != userInfo.Subject)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this problem"); 
        }
    }
}