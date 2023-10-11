//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Assertions;

internal static class AuthenticationResponseAssertions
{
    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        AuthenticationResult authResult)
    {
        var authResponse = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        authResponse.Should().NotBeNull();
        authResponse!.UserId.Should().Be(authResult.UserId);
        authResponse.UserName.Should().Be(authResult.UserName);
        authResponse.Token.Should().NotBeEmpty();
        authResponse.RefreshToken.Should().NotBeEmpty();
        authResponse.Expiration.Should().BeAfter(DateTime.UtcNow);
    }

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        RegisterRequest request)
    {
        var authResponse = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        authResponse.Should().NotBeNull();
        authResponse!.UserId.Should().NotBeEmpty();
        authResponse.UserName.Should().Be(request.UserName);
        authResponse.Token.Should().NotBeEmpty();
        authResponse.RefreshToken.Should().NotBeEmpty();
        authResponse.Expiration.Should().BeAfter(DateTime.UtcNow);
    }
}
