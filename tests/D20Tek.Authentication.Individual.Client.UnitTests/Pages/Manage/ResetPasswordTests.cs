//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages.Manage;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages.Manage;

[TestClass]
public class ResetPasswordTests
{
    [TestMethod]
    public void InitialRender_WithUnauthenticatedUser()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>().Object;

        using var ctx = new TestContext();
        ctx.AddTestAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService);

        // act
        var comp = ctx.RenderComponent<ResetPassword>();

        // assert
        var expectedHtml =
@"
<h4>Reset Password</h4>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control valid"">
        <label class=""form-label"" for=""email"">Email</label>
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
      <button id=""reset-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
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
    public void Submit_WithValidResetData()
    {
        // arrange
        var response = AuthorizationFactory.CreateAuthResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(response));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<ResetPassword>(p =>
            p.Add(x => x.ResetCode, "test-reset-code"));

        comp.Find("#email-input").Change("tester@test.com");
        comp.Find("#new-password-input").Change("NewPassword3$");
        comp.Find("#confirm-password-input").Change("NewPassword3$");
        comp.Find("#reset-submit").Click();

        // assert
        nav.Uri.Should().Be("http://localhost/");

        var expectedHtml =
@"
    <h4>Reset Password</h4>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""new-password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password""
               class=""form-control modified valid"" value=""NewPassword3$"">
        <label class=""form-label"" for=""new-password"">New Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password""
               class=""form-control modified valid"" value=""NewPassword3$""  >
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""reset-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Update password
      </button>
    </form>
    <div class=""my-2"">Password reset successfully!</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithAuthenticationServiceError()
    {
        // arrange
        var response = AuthorizationFactory.CreateAuthResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(
                       Error.NotFound("Test.Error", "Password not reset.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<ResetPassword>();
        comp.Find("#email-input").Change("tester@test.com");
        comp.Find("#new-password-input").Change("NewPassword3$");
        comp.Find("#confirm-password-input").Change("NewPassword3$");
        comp.Find("#reset-submit").Click();

        // assert
        var expectedHtml =
@"
<h4>Reset Password</h4>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""new-password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password""
               class=""form-control modified valid"" value=""NewPassword3$"">
        <label class=""form-label"" for=""new-password"">New Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password""
               class=""form-control modified valid"" value=""NewPassword3$""  >
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""reset-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Update password
      </button>
    </form>
    <div class=""my-2"">Error (Test.Error [3]): Password not reset.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
