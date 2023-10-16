//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public void AddAuthenticationPresentation_AddsExpectedTypes()
    {
        // arrange
        var services = new ServiceCollection();
        var mockConfig = CreateMockConfiguration();

        // act
        var result = services.AddAuthenticationPresentation(
            mockConfig.Object);

        // assert
        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThanOrEqualTo(20);
        var provider = services.BuildServiceProvider();
        provider.GetService<IOptions<JwtClientSettings>>().Should().NotBeNull();
        provider.GetService<IOptions<ServiceEndpointSettings>>().Should().NotBeNull();
        provider.GetService<IOptions<AuthenticationStateProvider>>().Should().NotBeNull();
        provider.GetService<IOptions<IAuthenticationService>>().Should().NotBeNull();
    }

    private Mock<IConfiguration> CreateMockConfiguration()
    {
        var mockSection = new Mock<IConfigurationSection>();

        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(x => x.GetSection(It.IsAny<string>()))
                  .Returns(mockSection.Object);

        return mockConfig;
    }
}
