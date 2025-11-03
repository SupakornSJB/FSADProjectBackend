using Duende.IdentityServer;
using Duende.IdentityServer.Models;

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

    public static IEnumerable<Client> Clients =>
    [
        // Machine-to-Machine client credentials flow client
        new Client
        {
            ClientId = "m2m.client",
            ClientName = "Client Credentials Client",

            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

            AllowedScopes = { "api1" }
        },

        // Interactive client, May not be required but added just in case
        new Client
        {
            ClientId = "web",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = { "https://localhost:5001/signin-oidc" },
            FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

            AllowOfflineAccess = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "api1"
            }
        },
        
        // React Frontend client 
        new Client
        {
            ClientId = "react-app",
            ClientName = "React SPA",

            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false, 
            RequirePkce = true, // Required for public clients / React client

            RedirectUris = { "http://localhost:5173/callback" }, 
            PostLogoutRedirectUris = { "http://localhost:5173" }, 
            AllowedCorsOrigins = { "http://localhost:5173" }, 

            AllowOfflineAccess = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "api1"
            }
        }
    ];
}