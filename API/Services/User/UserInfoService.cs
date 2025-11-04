using System.Security.Claims;
using FSADProjectBackend.Interfaces.User;
using Duende.IdentityModel.Client;
using FSADProjectBackend.Viewmodels.User;

namespace FSADProjectBackend.Services.User;

public class UserInfoService: IUserInfoService
{
    private readonly HttpClient _httpClient; 
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserInfoService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
       _httpClient = httpClient; 
       _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<IEnumerable<Claim>> GetUserInfo()
    {
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("No valid authorization token provided");
        }
        
        var accessToken = authHeader.Substring("Bearer ".Length).Trim();
        var disco = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:5000"); // Todo: Change to use environment variable

        if (disco.IsError)
        {
            throw new Exception(disco.Error);
        }

        var userInfo = await _httpClient.GetUserInfoAsync(new UserInfoRequest
        {
            Address = disco.UserInfoEndpoint,
            Token = accessToken
        });
        
        return userInfo.IsError ? throw new Exception(userInfo.Error) : userInfo.Claims;
    }

    public async Task<UserClaimsViewmodel> GetUserInfoAsUserClaimsVm()
    {
        var userInfo = await GetUserInfo();
        if (userInfo == null || !userInfo.Any()) 
        {
            throw new Exception("No user info found");    
        }
        
        return new UserClaimsViewmodel
        {
            Subject = userInfo.FirstOrDefault(x => x.Type == "sub")?.Value,
            Name = userInfo.FirstOrDefault(x => x.Type == "name")?.Value,
            FamilyName = userInfo.FirstOrDefault(x => x.Type == "family_name")?.Value,
            GivenName = userInfo.FirstOrDefault(x => x.Type == "given_name")?.Value,
            Website = userInfo.FirstOrDefault(x => x.Type == "website")?.Value,
            PreferredUsername = userInfo.FirstOrDefault(x => x.Type == "preferred_username")?.Value,
        };
    }
}