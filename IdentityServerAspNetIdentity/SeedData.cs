using System.Security.Claims;
using Duende.IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServerAspNetIdentity;

public static class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
        
        context.Database.EnsureCreated();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        CreateMockUser(
            userMgr,
            new ApplicationUser()
            {
                UserName = "alice",
                Email = "AliceSmith@example.com",
                EmailConfirmed = true,
            },
            [
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alice.example.com"),
            ]            ,
            "Pass123$"
        );
    
        CreateMockUser(
            userMgr,
            new ApplicationUser()
            {
                UserName = "bob",
                Email = "BobSmith@example.com",
                EmailConfirmed = true,
            }, 
            [
                new Claim(JwtClaimTypes.Name, "Bob Smith"),
                new Claim(JwtClaimTypes.GivenName, "Bob"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://bob.example.com"),
                new Claim("location", "somewhere")
            ],
            "Pass123$"
        );
    }

    public static void CreateMockUser(UserManager<ApplicationUser> userMgr, ApplicationUser newUser, Claim[] claims, string password)
    {
        if (newUser == null || string.IsNullOrWhiteSpace(newUser.UserName))
        {
            Log.Debug("No new user");
            return;
        }
        
        var user = userMgr.FindByNameAsync(newUser.UserName).Result;
        if (user == null)
        {
            var result = userMgr.CreateAsync(newUser, password).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(newUser, claims).Result;
            
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            
            Log.Debug($"{newUser.UserName} created");
        }
        else
        {
            Log.Debug($"{newUser.UserName} already exists");
        }
    }
}