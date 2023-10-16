//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public class SettingsTests
{
    [TestMethod]
    public void JwtSettingsProperties_GetSetValues()
    {
        // arrange

        // act
        var settings = new JwtClientSettings
        {
            Audience = "test-audience",
            Issuer = "test-issuer",
            Secret = "test-secret"
        };

        // assert
        settings.Should().NotBeNull();
        settings.Audience.Should().Be("test-audience");
        settings.Issuer.Should().Be("test-issuer");
        settings.Secret.Should().Be("test-secret");
    }

    [TestMethod]
    public void ServiceEndpointSettingsProperties_GetSetValues()
    {
        // arrange

        // act
        var settings = new ServiceEndpointSettings
        {
            Authentication = "test-auth-baseurl",
            ServiceBase = "test-service-baseurl"
        };

        // assert
        settings.Should().NotBeNull();
        settings.Authentication.Should().Be("test-auth-baseurl");
        settings.ServiceBase.Should().Be("test-service-baseurl");
    }
}
