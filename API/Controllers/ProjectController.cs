using FSADProjectBackend.Interfaces.Project;
using FSADProjectBackend.Models;
using FSADProjectBackend.Viewmodels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController: ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IUserProjectService _userProjectService;
    
    public ProjectController(
        IProjectService projectService,
        IUserProjectService userProjectService
        )
    {
        _projectService = projectService;
        _userProjectService = userProjectService;
    }
    
    [HttpGet]
    public IActionResult GetProjects([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        return new JsonResult(_projectService.GetProjects(page, pageSize));
    }

    [HttpGet("{id}")]
    public IActionResult GetProjectById(string id)
    {
        return new JsonResult(_projectService.GetProject(id));
    }

    [HttpPost]
    public IActionResult CreateNewProject([FromBody] CreateProjectViewmodel createProjectViewmodel)
    {
        return new JsonResult(_projectService.CreateProject(createProjectViewmodel));
    }

    [HttpGet("user")]
    public IActionResult GetUserInvolvedProjects([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        return new JsonResult(_userProjectService.GetProjectsOfUser(page,  pageSize));
    }

    [HttpGet("user/{userSubject}")]
    public IActionResult GetUserInvolvedProjects(string userSubject, [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null)
    {
        return new JsonResult(_userProjectService.GetProjectsOfUser(userSubject, page, pageSize));
    }
    
    [HttpGet("problemsolver/{id}")]
    public IActionResult GetProblemSolverInvolvedProjects(string id, [FromQuery] int? page = null,
        [FromQuery] int? pageSize = null)
    {
        return new JsonResult(_userProjectService.GetProjectsOfProblemSolvers(id, page, pageSize));
    }
        
    [HttpPut]
    public async Task<IActionResult> UpdateProject(Project project)
    {
        return new JsonResult(await _projectService.UpdateProject(project));
    }
}