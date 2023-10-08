//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Authentication.Individual.UseCases.Register;
using System.Net;

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
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        // act
        var response = await client.GetAsync("/api/v1/account");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
