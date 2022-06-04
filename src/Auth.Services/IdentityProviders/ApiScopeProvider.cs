using IdentityServer4.Models;

namespace Auth.Services.IdentityProviders;

public static class ApiScopeProvider
{
    public static IEnumerable<ApiScope> GetApiScopes() =>
        new ApiScope[]
        {
            new("excursions.read", "Read excursions"),
            new("excursions.write", "Write excursions"),
            new("payment.read", "Read payment"),
            new("payment.write", "Write payment"),
            new("files.read", "Read files"),
            new("files.write", "Write files")
        };
}