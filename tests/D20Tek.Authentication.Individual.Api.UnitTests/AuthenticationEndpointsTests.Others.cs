//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Json;
using System.Net;
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.UseCases.RefreshToken;

namespace D20Tek.Authentication.Individual.Api.UnitTests;

public partial class AuthenticationEndpointsTests
{
    [TestMethod]
    public async Task ChangeRoles_WithValidRole_ReturnsOk()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ChangeRole-1",
            email: "role1@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        var request = new ChangeRoleRequest("TestUser-ChangeRole-1", UserRoles.Admin);
        var endpoint = new AuthenticationEndpoints();
        var handler = _factory.GetChangeRoleCommandHandler();

        // act
        var response = await endpoint.ChangeRoleAsync(request, handler, CancellationToken.None);

        // assert
        response.Should().NotBeNull();
        var okResponse = response.As<Ok>();
        okResponse.Should().NotBeNull();
        okResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [TestMethod]
    public async Task ChangeRoles_WithValidationError_ReturnsBadRequest()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ChangeRole-2",
            email: "role2@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        var request = new ChangeRoleRequest("", UserRoles.User);
        var endpoint = new AuthenticationEndpoints();
        var handler = _factory.GetChangeRoleCommandHandler();

        // act
        var response = await endpoint.ChangeRoleAsync(request, handler, CancellationToken.None);

        // assert
        response.Should().NotBeNull();
        var typedResponse = response.As<ProblemHttpResult>();
        typedResponse.Should().NotBeNull();
        typedResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [TestMethod]
    public async Task ChangeRoles_WithInvalidRole_ReturnsBadRequest()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ChangeRole-3",
            email: "role3@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        var request = new ChangeRoleRequest("TestUser-ChangeRole-3", "PowerUser");
        var endpoint = new AuthenticationEndpoints();
        var handler = _factory.GetChangeRoleCommandHandler();

        // act
        var response = await endpoint.ChangeRoleAsync(request, handler, CancellationToken.None);

        // assert
        response.Should().NotBeNull();
        var typedResponse = response.As<ProblemHttpResult>();
        typedResponse.Should().NotBeNull();
        typedResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [TestMethod]
    public async Task RefreshToken_WithValidToken_ReturnsAuthenticationResponse()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateAuthenticatedClient(authResult.RefreshToken);
        var request = new RefreshTokenQuery(new Guid(authResult.UserId));

        // act
        var response = await client.PostAsJsonAsync<RefreshTokenQuery>(
            "/api/v1/account/token/refresh",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(authResult);
    }

    [TestMethod]
    public async Task RefreshToken_WithInvalidUserId_ReturnsNotFound()
    {
        // arrange
        var token = AuthTokenFactory.GenerateRefreshTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        // act
        var response = await client.PostAsync(
            "/api/v1/account/token/refresh",
            JsonContent.Create("", typeof(string)));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task RefreshToken_WithAccessToken_ReturnsForbidden()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = new RefreshTokenQuery(new Guid(authResult.UserId));

        // act
        var response = await client.PostAsJsonAsync<RefreshTokenQuery>(
            "/api/v1/account/token/refresh",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
