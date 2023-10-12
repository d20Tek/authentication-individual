//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages.Manage;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages.Manage;

[TestClass]
public class ForgotPasswordTests
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
        var comp = ctx.RenderComponent<ForgotPassword>();

        // assert
        var expectedHtml =
@"
<h4>Forgot your password</h4>
<p>Enter the email address associated with an account in this application.</p>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email""
               aria-required=""true"" placeholder=""name@example.com""
               class=""form-control valid"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <button id=""reset-password"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Reset Password
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
        var response = new ResetResponse("test-reset-code");
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetPasswordResetTokenAsync(It.IsAny<GetResetTokenRequest>()))
                   .Returns(Task.FromResult<Result<ResetResponse>>(response));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<ForgotPassword>();
        comp.Find("#email-input").Change("tester@test.com");
        comp.Find("#reset-password").Click();

        // assert
        var expectedHtml =
@"
    <h4>Forgot your password</h4>
<p>Enter the email address associated with an account in this application.</p>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <button id=""reset-password"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Reset Password
      </button>
    </form>
    <div class=""my-2""></div>
  </div>
</div>
<hr>
<div class=""my-2"">
  <p>This website hasn't integrated with a mail service yet, so your reset link is below:</p>
  <p>
    Please reset your password by
    <a href=""/authentication/reset-password/test-reset-code"">clicking here</a>.
  </p>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithAuthenticationServiceError()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetPasswordResetTokenAsync(It.IsAny<GetResetTokenRequest>()))
                   .Returns(Task.FromResult<Result<ResetResponse>>(
                       Error.NotFound("Test.Error", "Reset token failed.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<ForgotPassword>();
        comp.Find("#email-input").Change("tester@test.com");
        comp.Find("#reset-password").Click();

        // assert
        var expectedHtml =
@"
<h4>Forgot your password</h4>
<p>Enter the email address associated with an account in this application.</p>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <button id=""reset-password"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Reset Password
      </button>
    </form>
    <div class=""my-2"">Error (Test.Error [3]): Reset token failed.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
