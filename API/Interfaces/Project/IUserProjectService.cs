namespace FSADProjectBackend.Interfaces.Project;

public interface IUserProjectService
{
    public Task<IEnumerable<Models.Project>> GetProjectsOfUser(int? page = null, int? pageSize = null);

    public IEnumerable<Models.Project> GetProjectsOfUser(string userSubject, int? page = null,
        int? pageSize = null);

    public IEnumerable<Models.Project> GetProjectsOfProblemSolvers(string problemSolverId, int? page = null,
        int? pageSize = null);
}