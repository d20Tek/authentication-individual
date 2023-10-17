//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Authentication.Individual.UseCases.Register;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Api.UnitTests;

[TestClass]
public class AccountEndpointsTests
{
    private static readonly AuthenticationWebApplicationFactory _factory;
    private static readonly RegisterCommand _defaultCommand;
    private static readonly IJwtTokenGenerator _jwtTokenGenerator;

    static AccountEndpointsTests()
    {
        _factory = new AuthenticationWebApplicationFactory();
        _defaultCommand = AccountCommandFactory.CreateRegisterCommand("TestUser");
        _jwtTokenGenerator = _factory.GetJwtTokenGenerator();
    }

    [TestMethod]
    public async Task GetAccount_WithValidAccountId_ReturnsAccountResponse()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        // act
        var response = await client.GetAsync("/api/v1/account");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(_defaultCommand);
    }

    [TestMethod]
    public async Task GetAccount_WithInvalidAccountId_ReturnsNotFound()
    {
        // arrange
        var local = new AuthenticationWebApplicationFactory(byPassApiSettings: true);
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = local.CreateAuthenticatedClient(token);

        // act
        var response = await client.GetAsync("/api/v1/account");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task UpdateAccount_WithValidAccountId_ReturnsAccountResponse()
    {
        // arrange
        var localUser = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-Update-1",
            email: "t1@test.com");
        var authResult = await _factory.RegisterTestUser(localUser);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = CreateUpdateRequest();

        // act
        var response = await client.PutAsJsonAsync<UpdateAccountRequest>("/api/v1/account", request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(request);
    }

    [TestMethod]
    public async Task UpdateAccount_WithInvalidAccountId_ReturnsNotFound()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);
        var request = CreateUpdateRequest();

        // act
        var response = await client.PutAsJsonAsync<UpdateAccountRequest>("/api/v1/account", request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task UpdateAccount_WithInvalidRequestData_ReturnsBadRequest()
    {
        // arrange
        var localUser = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-Update-2",
            email: "t2@test.com");
        var authResult = await _factory.RegisterTestUser(localUser);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = new UpdateAccountRequest(
            "TestUser-Update-1",
            "",
            "UpdateLast",
            "foobar",
            "555-555-55");

        // act
        var response = await client.PutAsJsonAsync<UpdateAccountRequest>("/api/v1/account", request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task UpdateAccount_WithDuplicateUserName_ReturnsConflict()
    {
        // arrange
        _ = await _factory.RegisterTestUser(_defaultCommand);
        var localUser = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-Update-3",
            email: "t3@test.com");

        var authResult = await _factory.RegisterTestUser(localUser);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = CreateUpdateRequest("TestUser");

        // act
        var response = await client.PutAsJsonAsync<UpdateAccountRequest>("/api/v1/account", request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [TestMethod]
    public async Task RemoveAccount_WithValidAccountId_ReturnsAccountResponse()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-Delete-1",
            email: "d1@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        // act
        var response = await client.DeleteAsync("/api/v1/account");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(authResult.UserId);
    }

    [TestMethod]
    public async Task RemoveAccount_WithInvalidAccountId_ReturnsNotFound()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        // act
        var response = await client.DeleteAsync("/api/v1/account");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private UpdateAccountRequest CreateUpdateRequest(string userName = "TestUser-Update-1")
    {
        return new UpdateAccountRequest(
            userName,
            "UpdateFirst",
            "UpdateLast",
            "updated@test.com",
            "555-555-5555");
    }
}
