//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
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

    public static AuthenticationResponse CreateAuthResponse(
        string userId = "test-user-id",
        string userName = "TestUser",
        string token = "test-access-token",
        DateTime? expiration = null,
        string refreshToken = "test-refresh-token")
    {
        return new AuthenticationResponse(
            userId,
            userName,
            token,
            expiration ?? DateTime.UtcNow.AddHours(1),
            refreshToken);
    }
}
