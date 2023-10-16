//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using System.Net;

namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public partial class AuthenticationServiceTests
{
    [TestMethod]
    public async Task DeleteAccount_WithValueRequest_ReturnsAccountResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAccountResponse();
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);

        // act
        var result = await service.DeleteAccountAsync();

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().BeEquivalentTo(serviceResponse.UserId);
    }

    [TestMethod]
    public async Task DeleteAccount_WithValueRequest_ReturnsAccountError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpClientWithProblem(HttpStatusCode.NotFound);
        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);

        // act
        var result = await service.DeleteAccountAsync();

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetAccount_WithValueRequest_ReturnsAccountResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAccountResponse();
        var httpClient = HttpClientFactory.CreateHttpGetClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);

        // act
        var result = await service.GetAccountAsync();

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task GetAccount_WithValueRequest_ReturnsAccountError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpGetClientWithProblem(HttpStatusCode.NotFound);
        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);

        // act
        var result = await service.GetAccountAsync();

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task Register_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAuthResponse();
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new RegisterRequest
        {
            UserName = "TestUser",
            GivenName = "Tester",
            FamilyName = "McTest",
            Email = "tester@test.com",
            PhoneNumber = "555-555-5555",
            Password = "Password-123!",
            ConfirmPassword = "Password-123!"
        };

        // act
        var result = await service.RegisterAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task Register_WithValueRequest_ReturnsAuthError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpGetClientWithProblem(HttpStatusCode.NotFound);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new RegisterRequest
        {
            UserName = "TestUser",
            GivenName = "Tester",
            FamilyName = "McTest",
            Email = "tester@test.com",
            PhoneNumber = "555-555-5555",
            Password = "Password-123!",
            ConfirmPassword = "Password-123!"
        };

        // act
        var result = await service.RegisterAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task Login_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAuthResponse();
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new LoginRequest
        {
            UserName = "TestUser",
            Password = "Password-123!"
        };

        // act
        var result = await service.LoginAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task Login_WithValueRequest_ReturnsAuthError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpGetClientWithProblem(HttpStatusCode.NotFound);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new LoginRequest
        {
            UserName = "TestUser",
            Password = "Password-123!"
        };

        // act
        var result = await service.LoginAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task Update_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAccountResponse();
        var httpClient = HttpClientFactory.CreateHttpClientWithResponse(serviceResponse);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new UpdateProfileRequest
        {
            UserName = "TestUser",
            GivenName = "Tester",
            FamilyName = "McTest",
            Email = "tester@test.com",
            PhoneNumber = "555-555-5555"
        };

        // act
        var result = await service.UpdateAccountAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(serviceResponse);
    }

    [TestMethod]
    public async Task Update_WithValueRequest_ReturnsAuthError()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateHttpGetClientWithProblem(HttpStatusCode.NotFound);

        var service = AuthorizationFactory.CreateAuthenticationService(httpClient);
        var request = new UpdateProfileRequest
        {
            UserName = "TestUser",
            GivenName = "Tester",
            FamilyName = "McTest",
            Email = "tester@test.com",
            PhoneNumber = "555-555-5555"
        };

        // act
        var result = await service.UpdateAccountAsync(request);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}
