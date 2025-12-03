using System.Security.Claims;
using FSADProjectBackend.Interfaces.User;
using Duende.IdentityModel.Client;
using FSADProjectBackend.Viewmodels.User;
using Shared.Viewmodels;

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
        var disco = await _httpClient.GetDiscoveryDocumentAsync("https://upts-identityserver.supakorn-sjb.com"); // Todo: Change to use environment variable

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
            Role = userInfo.FirstOrDefault(x => x.Type == "role")?.Value,
        };
    }

    public async Task<PublicUserViewmodel> GetUserInfoAsUserClaimsVm(string subject)
    {
        await SetupIdentityHttpClient();
        var res = await _httpClient.GetAsync("https://upts-identityserver.supakorn-sjb.com/api/usermanager/" + subject); // Todo: Change to use environment variable
        var content = await res.Content.ReadFromJsonAsync<UserClaimsViewmodel>();
        
        if (content == null) throw new Exception("Cannot get user info"); 
        return new PublicUserViewmodel
        {
            Subject = content.Subject,
            Name = content.Name,
        };
    }
    
    private async Task SetupIdentityHttpClient()
    {
        var token = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = "https://upts-identityserver.supakorn-sjb.com/connect/token",            
            ClientId = "m2m.client",
            ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
            Scope = "api1"
        });
        
        if (token.IsError || token.AccessToken == null) throw new Exception(token.Error);
        _httpClient.SetBearerToken(token.AccessToken);
    }
}