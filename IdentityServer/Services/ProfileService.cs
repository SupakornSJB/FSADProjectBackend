using System.Security.Claims;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public class ProfileService: IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

    public ProfileService(UserManager<ApplicationUser> userManager, 
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
    {
        _userManager = userManager;
        _claimsFactory = claimsFactory;
    }
    
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        if (user == null)
        {
            return;
        }

        var principal = await _claimsFactory.CreateAsync(user);
        var identityClaims = principal.Claims.Select(x => new Claim(x.Type, x.Value)).ToList();

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(JwtClaimTypes.Role, r)).ToList();

        context.IssuedClaims.AddRange(identityClaims);
        context.IssuedClaims.AddRange(roleClaims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}