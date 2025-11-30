using FSADProjectBackend.Interfaces.Project;
using FSADProjectBackend.Models;

namespace FSADProjectBackend.Services.Project;

public class ProjectProgressUpdateService: IProjectProgressUpdateService
{
    public ProjectProgressUpdate GetAllProjectProgressUpdateOfProject(string projectId, int? page, int? pageSize)
    {
        throw new NotImplementedException();
    }

    public ProjectProgressUpdate GetProjectProgressUpdateOfProject(string projectId, string progressUpdateId)
    {
        throw new NotImplementedException();
    }

    public ProjectProgressUpdate CreateProjectProgressUpdate(string projectId, ProjectProgressUpdate update)
    {
        throw new NotImplementedException();
    }

    public ProjectProgressUpdate UpdateProjectProgressUpdate(string projectId, ProjectProgressUpdate update)
    {
        throw new NotImplementedException();
    }

    public ProjectProgressUpdate DeleteProjectProgressUpdate(string projectId, string projectUpdateId)
    {
        throw new NotImplementedException();
    }
}