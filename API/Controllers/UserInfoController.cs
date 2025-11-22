using FSADProjectBackend.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSADProjectBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserInfoController
{
    private readonly IUserInfoService _userInfoService;
    
    public UserInfoController(IUserInfoService userInfoService)
    {
        _userInfoService = userInfoService;    
    }
    
    [HttpGet] 
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        return new JsonResult(await _userInfoService.GetUserInfoAsUserClaimsVm());
    }
}