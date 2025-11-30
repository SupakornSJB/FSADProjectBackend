namespace IdentityServer.Settings;

public class ClientSettings
{
    public string ClientId { get; set; }
    public string ClientName { get; set; }

    public List<string> AllowedGrantTypes { get; set; }
    public List<string> ClientSecrets { get; set; }

    public List<string> RedirectUris { get; set; }
    public List<string> PostLogoutRedirectUris { get; set; }
    public string FrontChannelLogoutUri { get; set; }

    public List<string> AllowedScopes { get; set; }
    public List<string> AllowedCorsOrigins { get; set; }

    public bool RequireClientSecret { get; set; } = true;
    public bool RequirePkce { get; set; } = false;
    public bool AllowOfflineAccess { get; set; } = false;
}