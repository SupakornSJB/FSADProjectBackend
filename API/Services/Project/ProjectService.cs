using FSADProjectBackend.Contexts;
using FSADProjectBackend.Interfaces.Problem;
using FSADProjectBackend.Interfaces.ProblemSolver;
using FSADProjectBackend.Interfaces.Project;
using FSADProjectBackend.Viewmodels.Project;

namespace FSADProjectBackend.Services.Project;

public class ProjectService: IProjectService
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly IProblemSolverMemberService _problemSolverMemberService;
    private readonly IProblemService _problemService;
    
    public ProjectService(
        MongoDbContext mongoDbContext, 
        IProblemSolverMemberService problemSolverMemberService,
        IProblemService problemService)
    {
        _mongoDbContext = mongoDbContext;
        _problemSolverMemberService = problemSolverMemberService;
        _problemService = problemService;
    }
    
    public IEnumerable<Models.Project> GetProjects(int? page = null, int? pageSize = null)
    {
        if ((page != null && pageSize == null) || (page == null && pageSize != null))
        {
            throw new Exception("Please provide both page and page size");
        }

        if (page == null || pageSize == null)
        {
            return _mongoDbContext.Projects;
        }

        return _mongoDbContext.Projects
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value);
    }

    public async Task<Models.Project?> GetProject(string id)
    {
        return await _mongoDbContext.Projects.FindAsync(id);
    }

    public async Task<Models.Project> CreateProject(CreateProjectViewmodel createProjectViewmodel)
    {
        await _problemSolverMemberService.CheckIfUserBelongInProblemSolverGroup(createProjectViewmodel.ProblemSolverId);
        await CheckCreatePermission(createProjectViewmodel.RelatedProblemId, createProjectViewmodel.RelatedSolutionId);
        
        var newEntity = _mongoDbContext.Projects.Add(
            new Models.Project
            {
                Name = createProjectViewmodel.Project.Name,
                Description = createProjectViewmodel.Project.Description,
                BannerPicture = createProjectViewmodel.Project.BannerPicture,
                RelatedProblemId = createProjectViewmodel.RelatedProblemId,
                RelatedSolutionId = createProjectViewmodel.RelatedSolutionId,
                ProblemSolverId = createProjectViewmodel.ProblemSolverId
            });
        
        await _mongoDbContext.SaveChangesAsync();
        return newEntity.Entity;
    }

    public async Task<Models.Project> UpdateProject(Models.Project project)
    {
        var permission = await _problemSolverMemberService.CheckIfUserBelongInProblemSolverGroup(project.ProblemSolverId);
        if (!permission)
        {
            throw new UnauthorizedAccessException("Cannot update project");
        }

        var selectedProject = await _mongoDbContext.Projects.FindAsync(project.Id);
        if (selectedProject == null)
        {
            throw new Exception("Project does not exist");
        }

        selectedProject.Name = project.Name;
        selectedProject.Description = project.Description;
        selectedProject.BannerPicture = project.BannerPicture;
        await _mongoDbContext.SaveChangesAsync();
        
        return selectedProject;
    }

    public async Task<Models.Project?> DeleteProject(string projectId)
    {
        var project = await _mongoDbContext.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new Exception("Cannot find project with this id");
        }

        _mongoDbContext.Remove(project);
        await _mongoDbContext.SaveChangesAsync();
        return project;
    }

    private async Task CheckCreatePermission(string? relatedProblemId, string? relatedSolutionId)
    {
        if (relatedProblemId == null) return;
        
        var problem = await _problemService.GetProblemById(relatedProblemId);
        if (problem == null)
        {
            throw new Exception("Problem does not exist");
        }

        var solution = problem.Solutions.FirstOrDefault(x => x.Id.ToString() == relatedSolutionId);
        if (solution == null)
        {
            throw new Exception("Specified solution does not exist");
        }
    }
}