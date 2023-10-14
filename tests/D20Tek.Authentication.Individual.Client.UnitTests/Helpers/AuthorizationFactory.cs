//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using D20Tek.Authentication.Individual.Client.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

    public static AuthenticationService CreateAuthenticationService(
        HttpClient httpClient)
    {
        var jwtSettings = new JwtClientSettings
        {
            Audience = "d20Tek.Auth.Sample",
            Issuer = "d20Tek.AuthenticationService",
            Secret = "d20Tek.Auth.Sample.Api.95B90643-7DBF-457D-B393-F02F9EA138C2"
        };
        var jwtOptions = new Mock<IOptions<JwtClientSettings>>();
        jwtOptions.Setup(x => x.Value).Returns(jwtSettings);

        SecurityToken validatedToken;
        var mockTokenHandler = new Mock<JwtSecurityTokenHandler>();
        mockTokenHandler.Setup(x => x.ValidateToken(
                            It.IsAny<string>(),
                            It.IsAny<TokenValidationParameters>(),
                            out validatedToken))
                        .Returns(ClaimsPrincipalFactory.CreateAuthenticatedPrincipal());

        var mockStorage = new Mock<ILocalStorageService>();
        var loggerProvider = new Mock<ILogger<JwtAuthenticationProvider>>().Object;
        var stateProvider = new JwtAuthenticationProvider(
            httpClient,
            jwtOptions.Object,
            mockStorage.Object,
            mockTokenHandler.Object,
            loggerProvider);

        var endpointSettings = new ServiceEndpointSettings
        {
            Authentication = "https://localhost:7296/api/v1/account",
            ServiceBase = "https://localhost:7296"
        };
        var options = new Mock<IOptions<ServiceEndpointSettings>>();
        options.Setup(x => x.Value).Returns(endpointSettings);

        var loggerService = new Mock<ILogger<AuthenticationService>>().Object;
    
        return new AuthenticationService(
            httpClient,
            stateProvider,
            mockStorage.Object,
            options.Object,
            loggerService);
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

    public static AccountResponse CreateAccountResponse(
        string userId = "test-user-id",
        string userName = "TestUser",
        string givenName = "Tester",
        string familyName = "McTest",
        string email = "tester@test.com",
        string phone = "555-555-5555")
    {
        return new AccountResponse(
            userId,
            userName,
            givenName,
            familyName,
            email,
            phone);
    }
}
