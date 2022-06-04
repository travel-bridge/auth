using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace Auth.Services.IdentityProviders;

public static class TestUserProvider
{
    public static List<TestUser> GetTestUsers() =>
        new()
        {
            new TestUser
            {
                SubjectId = "52690dd2-38cd-48dd-b3c8-45bc098780f5",
                Username = "test",
                Password = "test",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Test Test"),
                    new Claim(JwtClaimTypes.GivenName, "Test"),
                    new Claim(JwtClaimTypes.FamilyName, "Test"),
                    new Claim(JwtClaimTypes.Email, "test-test@test.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
                }
            },
        };
}