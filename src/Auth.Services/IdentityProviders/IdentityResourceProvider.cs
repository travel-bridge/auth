using IdentityServer4.Models;

namespace Auth.Services.IdentityProviders;

public static class IdentityResourceProvider
{
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone()
        };
}