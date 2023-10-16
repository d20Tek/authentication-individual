//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public class JwtAuthenticationProviderTests
{
    [TestMethod]
    public async Task GetAuthenticationStateAsync_WithNoLocalStorageEntry_ReturnsAnonymous()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateEmptyHttpClient();
        var provider = AuthorizationFactory.CreateJwtAuthenticationProvider(httpClient);

        // act
        var result = await provider.GetAuthenticationStateAsync();

        // assert
        result.Should().NotBeNull();
        result.User.Identity.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAuthenticationStateAsync_WithLocalStorageEntry_ReturnsUser()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateEmptyHttpClient();
        var provider = AuthorizationFactory.CreateJwtAuthenticationProvider(
            httpClient,
            "test-access-token");

        // act
        var result = await provider.GetAuthenticationStateAsync();

        // assert
        result.Should().NotBeNull();
        result.User.Identity.Should().NotBeNull();
        result.User.Identity!.IsAuthenticated.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetAuthenticationStateAsync_WithDecodeError_ReturnsAnonymous()
    {
        // arrange
        var httpClient = HttpClientFactory.CreateEmptyHttpClient();
        var provider = AuthorizationFactory.CreateJwtAuthenticationProvider(
            httpClient,
            "test-access-token",
            false);

        // act
        var result = await provider.GetAuthenticationStateAsync();

        // assert
        result.Should().NotBeNull();
        result.User.Identity.Should().BeNull();
    }
}
