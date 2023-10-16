//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Pages;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages;

[TestClass]
public class ShowClaimsTests
{
    [TestMethod]
    public void Render_WithAuthenticatedUser_ShowsClaimsTable()
    {
        // arrange
        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();

        // act
        var comp = ctx.RenderComponent<ShowClaims>();

        // assert
        var expectedHtml =
@"<h3>Access Token Claims</h3>
<table class=""table"">
  <thead>
    <tr>
      <th>Claim Type</th>
      <th>Value</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name</td>
      <td>TestUser</td>
    </tr>
    <tr>
      <td>http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid</td>
      <td>518e2417-6c51-4f3d-9d4c-51367b7619bc</td>
    </tr>
    <tr>
      <td>http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress</td>
      <td>tester@test.com</td>
    </tr>
  </tbody>
</table>";

        comp.MarkupMatches(expectedHtml);
    }
}
