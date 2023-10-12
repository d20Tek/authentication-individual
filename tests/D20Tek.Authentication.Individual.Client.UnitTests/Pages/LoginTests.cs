//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages;
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages;

[TestClass]
public class LoginTests
{
    [TestMethod]
    public void Render_InitialLogin()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>().Object;

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService);

        // act
        var comp = ctx.RenderComponent<Login>();

        // assert
        var expectedHtml =
@"
<h3>Log in</h3>
<div class=""row my-2"">
  <div class=""col-md-4"">
    <section>
      <form >
        <div class=""form-floating my-2"">
          <input id=""username-input"" autocomplete=""username"" aria-required=""true"" placeholder=""UserName"" class=""form-control valid""  >
          <label class=""form-label"" for=""UserName"">User Name</label>
        </div>
        <div class=""form-floating my-2"">
          <input id=""password-input"" type=""password"" autocomplete=""current-password"" aria-required=""true"" class=""form-control valid""  >
          <label class=""form-label"" for=""Password"">Password</label>
        </div>
        <div class=""my-2""></div>
        <div class=""my-2"">
          <button id=""login-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
            Log in
          </button>
        </div>
        <hr>
        <div>
          <p>
            <a id=""forgot-password"" href=""./authentication/forgot-password"">
              Forgot your password?
            </a>
          </p>
          <p>
            <a href=""./authentication/register"">Register as new user</a>
          </p>
        </div>
      </form>
    </section>
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
        authService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(response));

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<Login>();
        comp.Find("#username-input").Change("TestUser");
        comp.Find("#password-input").Change("Password123!");
        comp.Find("#login-submit").Click();

        // assert
        nav.Uri.Should().Be("http://localhost/");

        var expectedHtml =
@"
    <h3>Log in</h3>
<div class=""row my-2"">
  <div class=""col-md-4"">
    <section>
      <form >
        <div class=""form-floating my-2"">
          <input id=""username-input"" autocomplete=""username"" aria-required=""true""
                 placeholder=""UserName"" class=""form-control modified valid"" value=""TestUser"">
          <label class=""form-label"" for=""UserName"">User Name</label>
        </div>
        <div class=""form-floating my-2"">
          <input id=""password-input"" type=""password"" autocomplete=""current-password""
                 aria-required=""true"" class=""form-control modified valid"" value=""Password123!"">
          <label class=""form-label"" for=""Password"">Password</label>
        </div>
        <div class=""my-2"">Wait...</div>
        <div class=""my-2"">
          <button id=""login-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
            Log in
          </button>
        </div>
        <hr>
        <div>
          <p>
            <a id=""forgot-password"" href=""./authentication/forgot-password"">
              Forgot your password?
            </a>
          </p>
          <p>
            <a href=""./authentication/register"">Register as new user</a>
          </p>
        </div>
      </form>
    </section>
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
        authService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(
                       Error.NotFound("Test.Error", "User  not found.")));

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<Login>();
        comp.Find("#username-input").Change("TestUser");
        comp.Find("#password-input").Change("WrongPassword!");
        comp.Find("#login-submit").Click();

        // assert
        var expectedHtml =
@"
    <h3>Log in</h3>
<div class=""row my-2"">
  <div class=""col-md-4"">
    <section>
      <form >
        <div class=""form-floating my-2"">
          <input id=""username-input"" autocomplete=""username"" aria-required=""true""
                 placeholder=""UserName"" class=""form-control modified valid"" value=""TestUser"">
          <label class=""form-label"" for=""UserName"">User Name</label>
        </div>
        <div class=""form-floating my-2"">
          <input id=""password-input"" type=""password"" autocomplete=""current-password""
                 aria-required=""true"" class=""form-control modified valid"" value=""WrongPassword!"">
          <label class=""form-label"" for=""Password"">Password</label>
        </div>
            <div class=""my-2"">Error (Test.Error [3]): User  not found.</div>
        <div class=""my-2"">
          <button id=""login-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
            Log in
          </button>
        </div>
        <hr>
        <div>
          <p>
            <a id=""forgot-password"" href=""./authentication/forgot-password"">
              Forgot your password?
            </a>
          </p>
          <p>
            <a href=""./authentication/register"">Register as new user</a>
          </p>
        </div>
      </form>
    </section>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
