namespace FSADProjectBackend.Interfaces.Tag;

public interface ITagService
{
    public IEnumerable<Models.Tag> GetTags(int? page = null, int? pageSize = null);
    public IEnumerable<Models.Tag> GetTagsByProblemId(string problemId);
    public Task UpdateProblemTags(string problemId, IEnumerable<string> tagNames);
}