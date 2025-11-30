using System.Security.Claims;
using FSADProjectBackend.Viewmodels.User;
using Shared.Viewmodels;

namespace FSADProjectBackend.Interfaces.User;

public interface IUserInfoService
{
    public Task<IEnumerable<Claim>> GetUserInfo();
    public Task<UserClaimsViewmodel> GetUserInfoAsUserClaimsVm();
    public Task<PublicUserViewmodel> GetUserInfoAsUserClaimsVm(string subject);
}