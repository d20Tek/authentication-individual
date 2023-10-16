//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using System.Net;

namespace D20Tek.Authentication.Individual.Client.UnitTests;

public partial class AuthenticationServiceTests
{
    [TestMethod]
    public async Task ChangePassword_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAuthResponse();
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "old-password",
            NewPassword = "new-password",
            ConfirmPassword = "new-password"
        };

        // act
        var result = await service.ChangePasswordAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task ChangePassword_WithValueRequest_ReturnsAuthError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpClientWithProblem(HttpStatusCode.NotFound);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "old-password",
            NewPassword = "new-password",
            ConfirmPassword = "new-password"
        };

        // act
        var result = await service.ChangePasswordAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.ValueOrDefault.Should().BeNull();
    }

    [TestMethod]
    public async Task GetPasswordResetToken_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = new ResetResponse("test-reset-token");
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new GetResetTokenRequest
        {
            Email = "tester@test.com"
        };

        // act
        var result = await service.GetPasswordResetTokenAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task GetPasswordResetToken_WithValueRequest_ReturnsAuthError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpClientWithProblem(HttpStatusCode.NotFound);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new GetResetTokenRequest
        {
            Email = "tester@test.com"
        };

        // act
        var result = await service.GetPasswordResetTokenAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task ResetPassword_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAuthResponse();
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new ResetPasswordRequest
        {
            Email = "tester@test.com",
            ResetToken = "test-reset-token",
            NewPassword = "new-password",
            ConfirmPassword = "new-password"
        };

        // act
        var result = await service.ResetPasswordAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task ResetPassword_WithValueRequest_ReturnsAuthError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpClientWithProblem(HttpStatusCode.NotFound);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new ResetPasswordRequest
        {
            Email = "tester@test.com",
            ResetToken = "test-reset-token",
            NewPassword = "new-password",
            ConfirmPassword = "new-password"
        };

        // act
        var result = await service.ResetPasswordAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}
