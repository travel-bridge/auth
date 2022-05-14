using IdentityServer4.Models;

namespace Auth.Services.Data;

public static class ApiScopeProvider
{
    public static IEnumerable<ApiScope> GetApiScopes() =>
        new ApiScope[]
        {
            new("excursions.read", "Read excursions"),
            new("excursions.write", "Write excursions"),
            new("payment.read", "Read payment"),
            new("payment.write", "Write payment")
        };
}