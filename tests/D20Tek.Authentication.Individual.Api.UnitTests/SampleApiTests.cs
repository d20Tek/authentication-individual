//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Auth.Sample.Api.Endpoints;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Authentication.Individual.UseCases.Register;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Api.UnitTests;

[TestClass]
public class SampleApiTests
{
    private static readonly AuthenticationWebApplicationFactory _factory;
    private static readonly RegisterCommand _defaultCommand;

    static SampleApiTests()
    {
        _factory = new AuthenticationWebApplicationFactory();
        _defaultCommand = AccountCommandFactory.CreateRegisterCommand("TestUser");
    }

    [TestMethod]
    public async Task GetHome_ReturnsPlainText()
    {
        // arrange
        using var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var message = await response.Content.ReadAsStringAsync();
        message.Should().Be("Authentication Sample WebApi");
    }

    [TestMethod]
    public async Task GetWeatherForecast_WithAuthenticatedUser_ReturnsWeatherResponse()
    {
        // arrange
        var authResult = await _factory.RegisterTestUser(_defaultCommand);
        using var client = _factory.CreateAuthenticatedClient(authResult.Token);

        // act
        var response = await client.GetAsync("/api/v1/weatherforecast");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var forecast = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
        forecast.Should().NotBeNull();
        forecast.Should().HaveCount(5);
    }

    [TestMethod]
    public async Task GetWeatherForecast_WithAnonymousUser_ReturnsUnauthorized()
    {
        // arrange
        using var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/api/v1/weatherforecast");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public void WeatherForecast_Setters()
    {
        // arrange
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var degrees = 10;
        var summary = "test";

        var forecast = new WeatherForecast(new DateOnly(), 0, "");

        // act
        forecast = forecast with
        {
            Date = date,
            TemperatureC = degrees,
            Summary = summary
        };

        // assert
        forecast.Should().NotBeNull();
        forecast.Date.Should().Be(date);
        forecast.TemperatureC.Should().Be(degrees);
        forecast.Summary.Should().Be(summary);
    }
}
