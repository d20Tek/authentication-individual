//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Components;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Components;

[TestClass]
public class RedirectToLoginTests
{
    [TestMethod]
    public void Render_WithUnauthenticatedUser_RedirectsToLoginPage()
    {
        // arrange
        using var ctx = new TestContext();
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<RedirectToLogin>();

        // assert
        nav.Uri.Should().Be("http://localhost/authentication/login");
    }
}
