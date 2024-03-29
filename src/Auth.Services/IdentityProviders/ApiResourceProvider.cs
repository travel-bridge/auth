using IdentityServer4.Models;

namespace Auth.Services.IdentityProviders;

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
            },
            new("files", "Files")
            {
                Scopes = new[]
                {
                    "files.read",
                    "files.write"
                }
            },
            new("profile", "Profile")
            {
                Scopes = new[]
                {
                    "profile.read",
                    "profile.write"
                }
            }
        };
}