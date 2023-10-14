//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Minimal.Result.Client;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public class AuthenticationServiceTests
{
    [TestMethod]
    public async Task ChangePassword_WithValueRequest_ReturnsAuthResponse()
    {
        // arrange
        var serviceResponse = AuthorizationFactory.CreateAuthResponse();
        var httpClient = CreateHttpClientWithResponse(serviceResponse);

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
        var httpClient = CreateHttpClientWithProblem(HttpStatusCode.NotFound);

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

    private HttpClient CreateHttpClientWithResponse<T>(
        T response,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var message = new HttpResponseMessage(statusCode);
        message.Content = JsonContent.Create(response);

        var httpClient = new Mock<HttpClient>();
        httpClient.Setup(x => x.SendAsync(
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()))
                  .ReturnsAsync(message);

        return httpClient.Object;
    }

    private HttpClient CreateHttpClientWithProblem(
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Detail = "test error",
            Title = "test-error",
            Type = "test-type"
        };

        return CreateHttpClientWithResponse(problem, statusCode);
    }
}
