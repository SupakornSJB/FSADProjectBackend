using Duende.IdentityServer.Models;
using IdentityServer.Settings;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new ApiScope(name: "api1", displayName: "Api 1")
    ];

    public static IEnumerable<Client> Clients(ClientSettings[] clientSettings)
    {
        return clientSettings.Select(c => new Client
        {
            ClientId = c.ClientId,
            ClientName = c.ClientName,

            AllowedGrantTypes = c.AllowedGrantTypes?.ToList() ?? GrantTypes.Code,
            ClientSecrets = c.ClientSecrets?
                .Select(s => new Secret(s.Sha256()))
                .ToList() ?? new List<Secret>(),

            RedirectUris = c.RedirectUris ?? [],
            PostLogoutRedirectUris = c.PostLogoutRedirectUris ?? [],
            FrontChannelLogoutUri = c.FrontChannelLogoutUri,

            AllowedScopes = c.AllowedScopes ?? [],

            AllowedCorsOrigins = c.AllowedCorsOrigins ?? [],

            RequirePkce = c.RequirePkce,
            RequireClientSecret = c.RequireClientSecret,
            AllowOfflineAccess = c.AllowOfflineAccess
        });
    }
}