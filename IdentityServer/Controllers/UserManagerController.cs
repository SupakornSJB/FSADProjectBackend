using System.Security.Claims;
using Duende.IdentityServer.Services;
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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IProfileService _profileService;

    public UserManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IProfileService profileService)
    {
        _userManager = userManager; 
        _roleManager = roleManager;
        _profileService = profileService;
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
    
    [HttpPut("{subject}")] 
    public async Task<IActionResult> UpdateUser(string subject, [FromBody] Dictionary<string, string> user)
    {
        var currentUser = await _userManager.FindByIdAsync(subject);
        if (currentUser == null)
        {
            throw new Exception("User not found");
        }

        foreach (var (key, value) in user)
        {
            var existingClaim = (await _userManager.GetClaimsAsync(currentUser))
                .FirstOrDefault(x => x.Type == key);

            if (existingClaim != null)
            {
                await _userManager.ReplaceClaimAsync(currentUser, existingClaim, new Claim(key, value));
            }
        }
        
        return Ok();
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        return new JsonResult(_userManager.Users);
    }
}