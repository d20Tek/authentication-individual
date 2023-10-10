using D20Tek.Authentication.Individual.Abstractions;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

internal static class AuthTokenFactory
{
    public static string GenerateTokenForRandomUser(IJwtTokenGenerator tokenGenerator)
    {
        var account = new UserAccount
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "TestUserFoo",
            GivenName = "first",
            FamilyName = "last",
            Email = "foo@bar.com"
        };

        var token = tokenGenerator.GenerateAccessToken(account, new string[] { "user" });
        return token.Token;
    }

    public static string GenerateRefreshTokenForRandomUser(IJwtTokenGenerator tokenGenerator)
    {
        var account = new UserAccount
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "TestUserFoo",
            GivenName = "first",
            FamilyName = "last",
            Email = "foo2@bar.com"
        };

        var token = tokenGenerator.GenerateRefreshToken(account);
        return token.Token;
    }
}
