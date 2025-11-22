using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Tag;
using FSADProjectBackend.Models;

namespace FSADProjectBackend.Services.Tag;

public class TagService: ITagService
{
    private readonly PgDbContext _pgDbContext;

    public TagService(PgDbContext pgDbContext)
    {
        _pgDbContext = pgDbContext; 
    }

    public IEnumerable<Models.Tag> GetTags(int? page = null, int? pageSize = null)
    {
        if ((page == null && pageSize != null) || (page != null && pageSize == null))
        {
            throw new Exception("Please provide both page and page size or neither");
        }

        if (page == null || pageSize == null)
        {
            return _pgDbContext.Tags.OrderBy(x => x.Id);    
        }
        
        return _pgDbContext.Tags
            .OrderBy(x => x.Id)
            .Skip(page.Value - 1)
            .Take(pageSize.Value);
    }

    public IEnumerable<Models.Tag> GetTagsByProblemId(string problemId)
    {
        return _pgDbContext.ProblemTagMappings
            .Where(x => x.ProblemId == problemId)
            .Select(x => x.Tag)
            .Distinct()
            .ToList();   
    }

    private async Task<IEnumerable<Models.Tag>> CreateNewTags(string[] tagNames)
    {
        var newTags = tagNames.Select(x => new Models.Tag { Name = x });
        await _pgDbContext.Tags.AddRangeAsync();
        await _pgDbContext.SaveChangesAsync();
        return newTags;
    }

    public async Task UpdateProblemTags(string problemId, IEnumerable<string> tagNames)
    {
        var allExistingTags = GetTags().Select(x => x.Name);
        var tagNamesToAdd = tagNames.Except(allExistingTags);
        await CreateNewTags(tagNamesToAdd.ToArray());

        _pgDbContext.ProblemTagMappings.RemoveRange(_pgDbContext.ProblemTagMappings.Where(x => x.ProblemId == problemId));
        var allNewTags = SearchTagsByNames(tagNames).Select(x => new ProblemTagMapping
        {
            TagId = x.Id,
            ProblemId = problemId
        });
        await _pgDbContext.ProblemTagMappings.AddRangeAsync(allNewTags);
        await _pgDbContext.SaveChangesAsync();
    }
    
    public IEnumerable<Models.Tag> SearchTagsByNames(IEnumerable<string> tagNames)
    {
        return _pgDbContext.Tags.Where(x => tagNames.Contains(x.Name));
    }
}