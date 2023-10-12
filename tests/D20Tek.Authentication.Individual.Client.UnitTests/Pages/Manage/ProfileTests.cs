//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages.Manage;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages.Manage;

[TestClass]
public class ProfileTests
{
    [TestMethod]
    public void InitialRender_WithAuthenticatedUser()
    {
        // arrange
        var response = AuthorizationFactory.CreateAccountResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(response));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<Profile>();

        // assert
        var expectedHtml =
@"
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input autocomplete=""username"" aria-required=""true"" disabled=""""
               class=""form-control valid"" value=""TestUser"">
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""given-name-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control valid"" value=""Tester"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""family-name-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control valid"" value=""McTest"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control valid""
               value=""tester@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" aria-required=""true""
               placeholder=""Enter your phone number"" class=""form-control valid""
               value=""555-555-5555"">
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <button id=""update-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Save
      </button>
    </form>
    <div class=""my-2""></div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void InitialRender_WithGetAccountError()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(
                       Error.NotFound("Test.Error", "Account not found.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<Profile>();

        // assert
        var expectedHtml =
@"
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input autocomplete=""username"" aria-required=""true"" disabled=""""
               class=""form-control valid"">
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""given-name-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control valid"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""family-name-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control valid"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control valid"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" aria-required=""true""
               placeholder=""Enter your phone number"" class=""form-control valid"">
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <button id=""update-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Save
      </button>
    </form>
    <div class=""my-2"">Error (Test.Error [3]): Account not found.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithValidProfileData()
    {
        // arrange
        var response = AuthorizationFactory.CreateAccountResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(response));
        authService.Setup(x => x.UpdateAccountAsync(It.IsAny<UpdateProfileRequest>()))
                   .Returns(Task.FromResult<Result<AccountResponse>>(response));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<Profile>();
        comp.Find("#given-name-input").Change("TesterX");
        comp.Find("#family-name-input").Change("MxTest");
        comp.Find("#email-input").Change("testerx@test.com");
        comp.Find("#phone-input").Change("555-555-5556");
        comp.Find("#update-submit").Click();

        // assert
        var expectedHtml =
@"
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input autocomplete=""username"" aria-required=""true"" disabled=""""
               class=""form-control valid"" value=""TestUser""  >
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""given-name-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""TesterX"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""family-name-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""MxTest"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""testerx@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" aria-required=""true""
               placeholder=""Enter your phone number"" class=""form-control modified valid""
               value=""555-555-5556""  >
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <button id=""update-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Save
      </button>
    </form>
    <div class=""my-2"">Account profile saved.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Submit_WithAuthenticationServiceError()
    {
        // arrange
        var response = AuthorizationFactory.CreateAccountResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(response));
        authService.Setup(x => x.UpdateAccountAsync(It.IsAny<UpdateProfileRequest>()))
                   .Returns(Task.FromResult<Result<AccountResponse>>(
                       Error.NotFound("Test.Error", "Update account failed.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<Profile>();
        comp.Find("#given-name-input").Change("TesterX");
        comp.Find("#family-name-input").Change("MxTest");
        comp.Find("#email-input").Change("testerx@test.com");
        comp.Find("#phone-input").Change("555-555-5556");
        comp.Find("#update-submit").Click();

        // assert
        var expectedHtml =
@"
<div class=""row"">
  <div class=""col-md-4"">
    <form >
      <div class=""form-floating mb-3"">
        <input autocomplete=""username"" aria-required=""true"" disabled=""""
               class=""form-control valid"" value=""TestUser""  >
        <label class=""form-label"" for=""UserName"">User Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""given-name-input"" autocomplete=""first-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""TesterX"">
        <label class=""form-label"" for=""given-name"">First Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""family-name-input"" autocomplete=""last-name"" aria-required=""true""
               placeholder=""name"" class=""form-control modified valid"" value=""MxTest"">
        <label class=""form-label"" for=""family-name"">Last Name</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""email-input"" type=""email"" autocomplete=""email"" aria-required=""true""
               placeholder=""name@example.com"" class=""form-control modified valid""
               value=""testerx@test.com"">
        <label class=""form-label"" for=""email"">Email</label>
      </div>
      <div class=""form-floating mb-3"">
        <input id=""phone-input"" type=""tel"" autocomplete=""phone"" aria-required=""true""
               placeholder=""Enter your phone number"" class=""form-control modified valid""
               value=""555-555-5556""  >
        <label class=""form-label"" for=""phone"">Phone number</label>
      </div>
      <button id=""update-submit"" type=""submit"" class=""w-100 btn btn-lg btn-primary"">
        Save
      </button>
    </form>
        <div class=""my-2"">Error (Test.Error [3]): Update account failed.</div>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
