//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.Client;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Helpers;

internal class HttpClientFactory
{
    public static HttpClient CreateHttpClientWithResponse<T>(
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

    public static HttpClient CreateHttpClientWithProblem(
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

    public static HttpClient CreateHttpGetClientWithResponse<T>(
        T response,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var message = new HttpResponseMessage(statusCode);
        message.Content = JsonContent.Create(response);

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(message);

        var httpClient = new HttpClient(handler.Object);
        return httpClient;
    }

    public static HttpClient CreateHttpGetClientWithProblem(
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Detail = "test error",
            Title = "test-error",
            Type = "test-type"
        };

        return CreateHttpGetClientWithResponse(problem, statusCode);
    }
}
