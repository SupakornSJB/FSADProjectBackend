using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.Tag;
using FSADProjectBackend.Interfaces.User;
using FSADProjectBackend.Viewmodels.Problem;
using MongoDB.Bson;

namespace FSADProjectBackend.Services.Problem;

public class ProblemService: IProblemService
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly PgDbContext _pgDbContext;
    private readonly IUserInfoService _userInfoService;
    private readonly ITagService _tagService;
    
    public ProblemService(MongoDbContext mongoDbContext, IUserInfoService userInfoService, ITagService tagService, PgDbContext pgDbContext)
    {
       _mongoDbContext = mongoDbContext; 
       _userInfoService = userInfoService;
       _tagService =  tagService;
       _pgDbContext = pgDbContext;
    }
    
    public IEnumerable<Models.Problem> GetProblems()
    {
        return _mongoDbContext.Problems;
    }

    public IEnumerable<Models.Problem> FilterByPageAndPageSizes(IEnumerable<Models.Problem> problems, int? page, int? pageSize)
    {
        if ((page != null && pageSize == null) || (pageSize == null && page != null))
        {
            throw new Exception("Page and Page size must be set"); 
        }

        if (page == null || pageSize == null)
        {
            return problems;
        }
        
        return problems.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
    }

    public IEnumerable<Models.Problem> FilterByKeywords(IEnumerable<Models.Problem> problems, string keywordString)
    {
        var keywords = keywordString.Split(" ");
        return problems.Where(x =>
                    keywords.Any(kw => x.Name.Contains(kw)));
    }

    public IEnumerable<Models.Problem> FilterByTags(IEnumerable<Models.Problem> problems, string[] tagNames)
    {
        var tagIds = _tagService.SearchTagsByNames(tagNames).Select(x => x.Id);
        var problemIds = _pgDbContext.ProblemTagMappings
            .Where(x => tagIds.Contains(x.TagId))
            .Select(x => x.ProblemId)
            .Distinct();
        return problems.Where(x => problemIds.Contains(x.Id.ToString()));
    }
    
    public async Task<Models.Problem?> GetProblemById(string id)
    {
        var problem = await _mongoDbContext.Problems.FindAsync(new ObjectId(id));
        return problem;
    }

    public async Task<IEnumerable<Models.Problem>> GetProblemsByIds(string[] ids)
    {
        var objectIds = ids.Select(x => new ObjectId(x));
        return _mongoDbContext.Problems.Where(x => objectIds.Contains(x.Id));
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

    public async Task IncrementViewCount(Models.Problem problem)
    {
        problem.ViewCount++;
        await _mongoDbContext.SaveChangesAsync();
    }
}