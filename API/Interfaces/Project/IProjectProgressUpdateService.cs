using FSADProjectBackend.Models;

namespace FSADProjectBackend.Interfaces.Project;

public interface IProjectProgressUpdateService
{
    public ProjectProgressUpdate GetAllProjectProgressUpdateOfProject(string projectId, int? page, int? pageSize);
    public ProjectProgressUpdate GetProjectProgressUpdateOfProject(string projectId, string progressUpdateId);
    public ProjectProgressUpdate CreateProjectProgressUpdate(string projectId, ProjectProgressUpdate update);
    public ProjectProgressUpdate UpdateProjectProgressUpdate(string projectId, ProjectProgressUpdate update);
    public ProjectProgressUpdate DeleteProjectProgressUpdate(string projectId, string projectUpdateId);
}