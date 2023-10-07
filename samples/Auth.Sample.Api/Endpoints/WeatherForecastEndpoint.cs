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
        routeBuilder.MapGet("/api/v1/weatherforecast", GetWeatherForecasts)
            .WithName("GetWeatherForecast")
            .Produces<WeatherForecast>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization()
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

