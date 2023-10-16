//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages;
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages;

[TestClass]
public class RegisterTests
{
    [TestMethod]
    public void Render_InitialRegister()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>().Object;

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService);

        // act
        var comp = ctx.RenderComponent<Register>();

        // assert
        var expectedHtml =
@"
<h3>Register</h3>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <h4>Create a new account.</h4>
      <hr>
      <div class=""form-floating mb-3"">
        <input id=""username-input"" autocomplete=""username"" aria-required=""true""
               placeholder=""UserName"" class=""form-control valid"">
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" typeof=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control valid"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""givenname-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control valid"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""familyname-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control valid"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" 
               placeholder=""Enter your phone number"" class=""form-control valid"">
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password"" class=""form-control valid"">
        <label class=""form-label"" for=""new-password"">Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password"" class=""form-control valid"">
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""register-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Register
      </button>
    </form>
    <div class=""my-2""></div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithValidAccountData()
    {
        // arrange
        var response = AuthorizationFactory.CreateAuthResponse();

        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(response));

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<Register>();
        comp.Find("#username-input").Change("TestUser");
        comp.Find("#email-input").Change("tester@test.com");
        comp.Find("#givenname-input").Change("Tester");
        comp.Find("#familyname-input").Change("McTest");
        comp.Find("#password-input").Change("Password123!");
        comp.Find("#confirm-password-input").Change("Password123!");
        comp.Find("#register-submit").Click();

        // assert
        nav.Uri.Should().Be("http://localhost/");

        var expectedHtml =
@"
    <h3>Register</h3>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <h4>Create a new account.</h4>
      <hr>
      <div class=""form-floating mb-3"">
        <input id=""username-input"" autocomplete=""username"" aria-required=""true""
               placeholder=""UserName"" class=""form-control modified valid"" value=""TestUser"">
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" typeof=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""givenname-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""Tester"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""familyname-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""McTest"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" 
               placeholder=""Enter your phone number"" class=""form-control valid"">
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""Password123!""  >
        <label class=""form-label"" for=""new-password"">Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""Password123!""  >
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""register-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Register
      </button>
    </form>
    <div class=""my-2""></div>
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
        authService.Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
                   .Returns(Task.FromResult<Result<AuthenticationResponse>>(
                       Error.NotFound("Test.Error", "Cannot register user.")));

        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<Register>();
        comp.Find("#username-input").Change("TestUser");
        comp.Find("#email-input").Change("tester@test.com");
        comp.Find("#givenname-input").Change("Tester");
        comp.Find("#familyname-input").Change("McTest");
        comp.Find("#password-input").Change("Password123!");
        comp.Find("#confirm-password-input").Change("Password123!");
        comp.Find("#register-submit").Click();

        // assert
        var expectedHtml =
@"
    <h3>Register</h3>
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <h4>Create a new account.</h4>
      <hr>
      <div class=""form-floating mb-3"">
        <input id=""username-input"" autocomplete=""username"" aria-required=""true""
               placeholder=""UserName"" class=""form-control modified valid"" value=""TestUser"">
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" typeof=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""givenname-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""Tester"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""familyname-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""McTest"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" 
               placeholder=""Enter your phone number"" class=""form-control valid"">
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""password-input"" type=""password"" autocomplete=""new-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""Password123!""  >
        <label class=""form-label"" for=""new-password"">Password</label>
      </div>
      <div class=""form-floating my-2"">
        <input id=""confirm-password-input"" type=""password"" autocomplete=""confirm-password""
               aria-required=""true"" placeholder=""password"" class=""form-control modified valid""
               value=""Password123!""  >
        <label class=""form-label"" for=""confirm-password"">Confirm Password</label>
      </div>
      <button id=""register-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Register
      </button>
    </form>
    <div class=""my-2"">Error (Test.Error [3]): Cannot register user.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
