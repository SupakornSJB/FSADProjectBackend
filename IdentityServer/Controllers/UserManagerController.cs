using IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Viewmodels;

namespace IdentityServer.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class UserManagerController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager; 
    
    public UserManagerController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager; 
    }
    
    [HttpGet("{subject}")]
    public async Task<IActionResult> GetUserById(string subject)
    {
        var requestedUser = await _userManager.FindByIdAsync(subject);
        if (requestedUser == null)
            return NotFound();
        
        return Ok(new PublicUserViewmodel
        {
            Subject = requestedUser.Id,
            Name = requestedUser.UserName,
        });
    }

    [HttpGet("search/{searchTerm}")]
    public IActionResult SearchUserByName(string searchTerm)
    {
        return new JsonResult(_userManager.Users.Where(x => x.UserName.Contains(searchTerm)));
    }
}