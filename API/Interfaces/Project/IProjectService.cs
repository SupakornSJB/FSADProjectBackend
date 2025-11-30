using FSADProjectBackend.Viewmodels.Project;

namespace FSADProjectBackend.Interfaces.Project;

public interface IProjectService
{
    public IEnumerable<Models.Project> GetProjects(int? page = null, int? pageSize = null);
    public Task<Models.Project?> GetProject(string id);
    
    public Task<Models.Project> CreateProject(CreateProjectViewmodel project);

    public Task<Models.Project> UpdateProject(Models.Project project);
    public Task<Models.Project?> DeleteProject(string projectId);
}