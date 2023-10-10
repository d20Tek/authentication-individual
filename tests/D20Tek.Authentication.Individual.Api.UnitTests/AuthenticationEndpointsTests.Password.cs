//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Api.UnitTests;

public partial class AuthenticationEndpointsTests
{
    [TestMethod]
    public async Task ChangePassword_WithValidPasswords_ReturnsAuthenticationResponse()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ChangePassword-1",
            email: "change1@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = new ChangePasswordRequest("Password123!", "Password1234!");

        // act
        var response = await client.PatchAsJsonAsync<ChangePasswordRequest>(
            "/api/v1/account/password",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(authResult);
    }

    [TestMethod]
    public async Task ChangePassword_WithInvalidCurrentPassword_ReturnsBadRequest()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ChangePassword-2",
            email: "change2@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = new ChangePasswordRequest("Invalid123!", "Password1234!");

        // act
        var response = await client.PatchAsJsonAsync<ChangePasswordRequest>(
            "/api/v1/account/password",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task ChangePassword_WithInvalidNewPassword_ReturnsBadRequest()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ChangePassword-2",
            email: "change2@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);
        var request = new ChangePasswordRequest("", "foobar");

        // act
        var response = await client.PatchAsJsonAsync<ChangePasswordRequest>(
            "/api/v1/account/password",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task ChangePassword_WithInvalidUserId_ReturnsNotFound()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);
        var request = new ChangePasswordRequest("Password123!", "Password1234!");

        // act
        var response = await client.PatchAsJsonAsync<ChangePasswordRequest>(
            "/api/v1/account/password",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task ResetPassword_WithValidReset_ReturnsAuthenticationResponse()
    {
        // arrange
        var local = AccountCommandFactory.CreateRegisterCommand(
            "TestUser-ResetPassword-1",
            email: "reset1@test.com");
        var authResult = await _factory.RegisterTestUser(local);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        var resetRequest = new GetResetTokenRequest("reset1@test.com");
        var resetMessage = await client.PostAsJsonAsync<GetResetTokenRequest>(
            "/api/v1/account/password/reset",
            resetRequest);

        var resetResponse = await resetMessage.Content.ReadFromJsonAsync<ResetTokenResponse>();
        resetResponse.Should().NotBeNull();

        // act
        var request = new ResetPasswordRequest(
            "reset1@test.com",
            resetResponse!.ResetToken,
            "TestPassword55!");

        var response = await client.PatchAsJsonAsync<ResetPasswordRequest>(
            "/api/v1/account/password/reset",
            request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await response.ShouldBeEquivalentTo(authResult);
    }

    [TestMethod]
    public async Task GetResetPassword_WithNonexistingEmail_ReturnsNotFound()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        var resetRequest = new GetResetTokenRequest("reset99@test.com");

        // act
        var resetMessage = await client.PostAsJsonAsync<GetResetTokenRequest>(
            "/api/v1/account/password/reset",
            resetRequest);

        // assert
        resetMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetResetPassword_WithValidationError_ReturnsBadRequest()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        var resetRequest = new GetResetTokenRequest("");

        // act
        var resetMessage = await client.PostAsJsonAsync<GetResetTokenRequest>(
            "/api/v1/account/password/reset",
            resetRequest);

        // assert
        resetMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task ResetPassword_WithNonexistingEmail_ReturnsNotFound()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        var resetRequest = new ResetPasswordRequest(
            "reset99@test.com",
            "fake-reset-token",
            "NewPassword");

        // act
        var resetMessage = await client.PatchAsJsonAsync<ResetPasswordRequest>(
            "/api/v1/account/password/reset",
            resetRequest);

        // assert
        resetMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [TestMethod]
    public async Task ResetPassword_WithValidationErrors_ReturnsBadRequest()
    {
        // arrange
        var token = AuthTokenFactory.GenerateTokenForRandomUser(_jwtTokenGenerator);
        using var client = _factory.CreateAuthenticatedClient(token);

        var resetRequest = new ResetPasswordRequest(
            "reset99-test.com",
            "",
            "foo");

        // act
        var resetMessage = await client.PatchAsJsonAsync<ResetPasswordRequest>(
            "/api/v1/account/password/reset",
            resetRequest);

        // assert
        resetMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
