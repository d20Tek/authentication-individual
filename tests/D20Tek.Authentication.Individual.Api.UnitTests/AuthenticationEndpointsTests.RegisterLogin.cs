//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Authentication.Individual.UseCases.Register;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Api.UnitTests;

[TestClass]
public partial class AuthenticationEndpointsTests
{
    private static readonly AuthenticationWebApplicationFactory _factory;
    private static readonly RegisterCommand _defaultCommand;
    private static readonly IJwtTokenGenerator _jwtTokenGenerator;

    static AuthenticationEndpointsTests()
    {
        _factory = new AuthenticationWebApplicationFactory();
        _defaultCommand = AccountCommandFactory.CreateRegisterCommand("TestUser");
        _jwtTokenGenerator = _factory.GetJwtTokenGenerator();
    }

    [TestMethod]
    public async Task Login_WithValidNameAndPassword_ReturnsAuthenticationResponse()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateClient();
        var request = new LoginRequest("TestUser", "Password123!");

        // act
        var response = await client.PostAsJsonAsync<LoginRequest>(
            "/api/v1/account/login",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(authResult);
    }

    [TestMethod]
    public async Task Login_WithValidationError_ReturnsBadRequest()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateClient();
        var request = new LoginRequest("", "Fake123!");

        // act
        var response = await client.PostAsJsonAsync<LoginRequest>(
            "/api/v1/account/login",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Login_WithInvalidPassword_ReturnsBadRequest()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateClient();
        var request = new LoginRequest("TestUser", "Fake123!");

        // act
        var response = await client.PostAsJsonAsync<LoginRequest>(
            "/api/v1/account/login",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Login_WithInvalidUserName_ReturnsBadRequest()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateClient();
        var request = new LoginRequest("TestUser-Fake", "Password123!");

        // act
        var response = await client.PostAsJsonAsync<LoginRequest>(
            "/api/v1/account/login",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Register_WithNewUserData_ReturnsAuthenticationResponse()
    {
        // arrange
        using var client = _factory.CreateClient();
        var request = CreateRegisterRequest();

        // act
        var response = await client.PostAsJsonAsync<RegisterRequest>(
            "/api/v1/account",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        await response.ShouldBeEquivalentTo(request);
    }

    [TestMethod]
    public async Task Register_WithExistingUserName_ReturnsConflict()
    {
        // arrange
        _ = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateClient();
        var request = CreateRegisterRequest("TestUser");

        // act
        var response = await client.PostAsJsonAsync<RegisterRequest>(
            "/api/v1/account",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [TestMethod]
    public async Task Register_WithExistingEmail_ReturnsConflict()
    {
        // arrange
        _ = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateClient();
        var request = CreateRegisterRequest("TestUser-Register-2", "tester@test.com");

        // act
        var response = await client.PostAsJsonAsync<RegisterRequest>(
            "/api/v1/account",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [TestMethod]
    public async Task Register_WithValidationErrors_ReturnsConflict()
    {
        // arrange
        using var client = _factory.CreateClient();
        var request = CreateRegisterRequest("", "foobar.com");

        // act
        var response = await client.PostAsJsonAsync<RegisterRequest>(
            "/api/v1/account",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task GetClaims_WithValidAccessToken_ReturnsClaimsList()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        // act
        var response = await client.GetAsync("/api/v1/account/claims");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var claims = await response.Content.ReadFromJsonAsync<List<string>>();

        claims.Should().NotBeNull();
        claims.Should().HaveCount(12);
        claims.Should().Contain("name: TestUser");
    }

    private RegisterRequest CreateRegisterRequest(
        string userName = "TestUser-Register",
        string email = "r1@test.com")
    {
        return new RegisterRequest(
            userName,
            "Tester",
            "McTest",
            email,
            "Password123!",
            "555-555-5555");
    }
}
