//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages.Manage;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages.Manage;

[TestClass]
public class ChangePasswordTests
{
    [TestMethod]
    public void InitialRender_WithAuthenticatedUser()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>().Object;

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService);

        // act
        var comp = ctx.RenderComponent<ChangePassword>();

        // assert
        var expectedHtml =
@"
<h4>Change Password</h4>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating my-2"">
        <input id=""current-password-input"" type=""password"" autocomplete=""current-password""
               aria-required=""true"" placeholder=""password"" class=""form-control valid"">
        <label class=""form-label"" for=""current-password"">Current Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""new-password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password"" class=""form-control valid"">
        <label class=""form-label"" for=""new-password"">New Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password"" class=""form-control valid"">
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""change-password-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Update password
      </button>
    </form>
    <div class=""my-2""></div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithValidLoginData()
    {
        // arrange
        var response = AuthorizationFactory.CreateAuthResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.ChangePasswordAsync(It.IsAny<ChangePasswordRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(response));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<ChangePassword>();
        comp.Find("#current-password-input").Change("Password123!");
        comp.Find("#new-password-input").Change("NewPassword3$");
        comp.Find("#confirm-password-input").Change("NewPassword3$");
        comp.Find("#change-password-submit").Click();

        // assert
        nav.Uri.Should().Be("http://localhost/authentication/profile");

        var expectedHtml =
@"
<h4>Change Password</h4>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating my-2"">
        <input id=""current-password-input"" type=""password"" autocomplete=""current-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""Password123!"">
        <label class=""form-label"" for=""current-password"">Current Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""new-password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""NewPassword3$"">
        <label class=""form-label"" for=""new-password"">New Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
                value=""NewPassword3$"">
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""change-password-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Update password
      </button>
    </form>
    <div class=""my-2"">Password changed successfully!</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithAuthenticationServiceError()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.ChangePasswordAsync(It.IsAny<ChangePasswordRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(
                       Error.NotFound("Test.Error", "Password not changed.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<ChangePassword>();
        comp.Find("#current-password-input").Change("Password123!");
        comp.Find("#new-password-input").Change("NewPassword3$");
        comp.Find("#confirm-password-input").Change("NewPassword3$");
        comp.Find("#change-password-submit").Click();

        // assert
        var expectedHtml =
@"
<h4>Change Password</h4>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating my-2"">
        <input id=""current-password-input"" type=""password"" autocomplete=""current-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""Password123!"">
        <label class=""form-label"" for=""current-password"">Current Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""new-password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""NewPassword3$"">
        <label class=""form-label"" for=""new-password"">New Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
                value=""NewPassword3$"">
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""change-password-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Update password
      </button>
    </form>
        <div class=""my-2"">Error (Test.Error [3]): Password not changed.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
