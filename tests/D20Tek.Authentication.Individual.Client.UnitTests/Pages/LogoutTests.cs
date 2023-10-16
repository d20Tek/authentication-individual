//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Pages;
using Microsoft.AspNetCore.Components;
using Moq;

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
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<Logout>();

        // assert
        nav.Uri.Should().Be("http://localhost/");
    }
}
