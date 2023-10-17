//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages;

[TestClass]
public class LogoutTests
{
    [TestMethod]
    public void Render_WithAuthenticatedUser_LogsOutAndRedirectsHome()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>().Object;

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService);
        ctx.Services.AddSingleton<IOptions<AuthClientSettings>>(Options.Create(
            new AuthClientSettings { LogoutUrl = "/test-logout" }));

        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<Logout>();

        // assert
        nav.Uri.Should().Be("http://localhost/test-logout");
    }
}
