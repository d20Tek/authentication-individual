//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Auth.Sample.Api.Endpoints;

internal static class WeatherForecastEndpoint
{
    private static string[] _summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static IEndpointRouteBuilder MapWeatherForecastEndpoints(
        this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/weatherforecast", GetWeatherForecasts)
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        return routeBuilder;
    }

    private static IResult GetWeatherForecasts()
    {
        var forecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ))
            .ToArray();

        return TypedResults.Ok(forecasts);
    }
}

