//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Components;

[TestClass]
public class LoginDisplayTests
{
    [TestMethod]
    public void Render_WithUnauthenticatedUser_ShowsLoginButton()
    {
        // arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();

        // act
        var comp = ctx.RenderComponent<LoginDisplay>();

        // assert
        var expectedHtml =
@"<a class=""mx-2"" href=""/authentication/register"">Register</a>|
<a class=""mx-2"" href=""/authentication/login"">Log in</a>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithAuthenticatedUser_ShowsLogoutButton()
    {
        // arrange
        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();

        // act
        var comp = ctx.RenderComponent<LoginDisplay>();

        // assert
        var expectedHtml =
@"<a class=""mx-2"" href=""/authentication/profile"">
  Hello, TestUser!
</a>|
<a class=""mx-2"" href=""/authentication/logout"">Log out</a>
";

        comp.MarkupMatches(expectedHtml);
    }
}
