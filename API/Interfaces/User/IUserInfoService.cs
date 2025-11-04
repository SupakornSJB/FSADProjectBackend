using System.Security.Claims;
using FSADProjectBackend.Viewmodels.User;

namespace FSADProjectBackend.Interfaces.User;

public interface IUserInfoService
{
    public Task<IEnumerable<Claim>> GetUserInfo();
    public Task<UserClaimsViewmodel> GetUserInfoAsUserClaimsVm();
}