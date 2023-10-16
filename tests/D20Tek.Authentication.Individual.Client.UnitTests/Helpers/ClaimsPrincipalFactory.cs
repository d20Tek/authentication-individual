//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Helpers;

internal static class ClaimsPrincipalFactory
{
    public static ClaimsPrincipal CreateAuthenticatedPrincipal()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "test-name"),
            new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.GivenName, "given-name"),
            new Claim(ClaimTypes.Surname, "family-name"),
            new Claim(ClaimTypes.Email, "tester@test.com")
        };
        var identity = new ClaimsIdentity(claims, "JwtAuth");

        return new ClaimsPrincipal(identity);
    }
}
