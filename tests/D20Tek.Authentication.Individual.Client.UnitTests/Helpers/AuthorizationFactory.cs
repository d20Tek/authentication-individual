//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Helpers;

internal static class AuthorizationFactory
{
    public static TestAuthorizationContext AddSimpleAppAuthorization(
        this TestContext testContext)
    {
        var id = "518e2417-6c51-4f3d-9d4c-51367b7619bc";

        var authContext = testContext.AddTestAuthorization();
        authContext.SetAuthorized("TestUser");
        authContext.SetClaims(
            new Claim(ClaimTypes.Sid, id),
            new Claim(ClaimTypes.Email, "tester@test.com"));

        return authContext;
    }
}
