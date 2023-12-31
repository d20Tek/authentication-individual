﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace D20Tek.Authentication.Individual.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureAppSettings(configuration);

        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<JwtSecurityTokenHandler>();
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();

        return services;
    }

    private static IServiceCollection ConfigureAppSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtClientSettings();
        configuration.Bind(nameof(JwtClientSettings), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        var endpointSettings = new ServiceEndpointSettings();
        configuration.Bind(nameof(ServiceEndpointSettings), endpointSettings);
        services.AddSingleton(Options.Create(endpointSettings));

        var authClientSettings = new AuthClientSettings();
        configuration.Bind(nameof(AuthClientSettings), authClientSettings);
        services.AddSingleton(Options.Create(authClientSettings));

        return services;
    }
}
