using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SolutionController
{
    public Task<IActionResult> GetProposedSolutionOfUser(string userId)
    {
        throw new NotImplementedException();      
    }
    
    public Task<IActionResult> GetSolutionOfProblem()
    {
        throw new NotImplementedException();      
    }

    public Task<IActionResult> CreateSolution()
    {
        throw new NotImplementedException();      
    }
}