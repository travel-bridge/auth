using IdentityServer4.Models;

namespace Auth.Services.Data;

public static class ApiResourceProvider
{
    public static IEnumerable<ApiResource> GetApiResources() =>
        new ApiResource[]
        {
            new("excursions", "Excursions")
            {
                Scopes = new[]
                {
                    "excursions.read",
                    "excursions.write"
                }
            },
            new("payment", "Payment")
            {
                Scopes = new[]
                {
                    "payment.read",
                    "payment.write"
                }
            }
        };
}