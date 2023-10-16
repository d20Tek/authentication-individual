//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;
using D20Tek.Authentication.Individual.Client.Pages.Manage;
using Microsoft.AspNetCore.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Pages.Manage;

[TestClass]
public class PersonalDataTests
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
        var comp = ctx.RenderComponent<PersonalData>();

        // assert
        var expectedHtml =
@"
    <h4>Personal data</h4>
<div class=""row"">
  <div class=""col-md-6"">
    <p>
      Your account contains personal data that you have given us.
      This page allows you to download or delete that data.
    </p>
    <p>
      <strong>Deleting this data will permanently remove your account,
              and it cannot be recovered.</strong>
    </p>
    <div class=""row"">
      <div class=""col"">
        <button id=""download-button"" class=""btn btn-primary"" >
          Download
        </button>
      </div>
      <div class=""col"">
        <button id=""delete-button"" class=""btn btn-danger"" >
          Delete data and close account
        </button>
      </div>
    </div>
  </div>
  <div class=""col-12 mt-4""></div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void DownloadData_WithAuthenticatedUser()
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
        var comp = ctx.RenderComponent<PersonalData>();
        comp.Find("#download-button").Click();

        // assert
        var expectedHtml =
@"
    <h4>Personal data</h4>
<div class=""row"">
  <div class=""col-md-6"">
    <p>
      Your account contains personal data that you have given us.
      This page allows you to download or delete that data.
    </p>
    <p>
      <strong>Deleting this data will permanently remove your account,
              and it cannot be recovered.</strong>
    </p>
    <div class=""row"">
      <div class=""col"">
        <button id=""download-button"" class=""btn btn-primary"" >
          Download
        </button>
      </div>
      <div class=""col"">
        <button id=""delete-button"" class=""btn btn-danger"" >
          Delete data and close account
        </button>
      </div>
    </div>
  </div>
  <div class=""col-12 mt-4"">
    <textarea class=""col-12 col-md-6"" style=""height: 60vh"" readonly="""">
    {
        ""userId"": ""test-user-id"",
        ""userName"": ""TestUser"",
        ""givenName"": ""Tester"",
        ""familyName"": ""McTest"",
        ""email"": ""tester@test.com"",
        ""phoneNumber"": ""555-555-5555""
    }
    </textarea>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void DownloadData_WithAuthenticationServiceError()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.GetAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(
                       Error.NotFound("Test.Error", "Get Account data failed.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<PersonalData>();
        comp.Find("#download-button").Click();

        // assert
        var expectedHtml =
@"
<h4>Personal data</h4>
<div class=""row"">
  <div class=""col-md-6"">
    <p>
      Your account contains personal data that you have given us.
      This page allows you to download or delete that data.
    </p>
    <p>
      <strong>Deleting this data will permanently remove your account,
              and it cannot be recovered.</strong>
    </p>
    <div class=""row"">
      <div class=""col"">
        <button id=""download-button"" class=""btn btn-primary"" >
          Download
        </button>
      </div>
      <div class=""col"">
        <button id=""delete-button"" class=""btn btn-danger"" >
          Delete data and close account
        </button>
      </div>
    </div>
  </div>
  <div class=""col-12 mt-4"">
    <textarea class=""col-12 col-md-6"" style=""height: 60vh"" readonly="""">
        [Test.Error]: Get Account data failed.
    </textarea>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void DeleteAccount_WithAuthenticatedUser()
    {
        // arrange
        var response = AuthorizationFactory.CreateAccountResponse();
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.DeleteAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(response));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        // act
        var comp = ctx.RenderComponent<PersonalData>();
        comp.Find("#delete-button").Click();

        // assert
        nav.Uri.Should().Be("http://localhost/");
        var expectedHtml =
@"
    <h4>Personal data</h4>
<div class=""row"">
  <div class=""col-md-6"">
    <p>
      Your account contains personal data that you have given us.
      This page allows you to download or delete that data.
    </p>
    <p>
      <strong>Deleting this data will permanently remove your account,
              and it cannot be recovered.</strong>
    </p>
    <div class=""row"">
      <div class=""col"">
        <button id=""download-button"" class=""btn btn-primary"" >
          Download
        </button>
      </div>
      <div class=""col"">
        <button id=""delete-button"" class=""btn btn-danger"" >
          Delete data and close account
        </button>
      </div>
    </div>
  </div>
  <div class=""col-12 mt-4""></div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void DeleteAccount_WithAuthenticationServiceError()
    {
        // arrange
        var authService = new Mock<IAuthenticationService>();
        authService.Setup(x => x.DeleteAccountAsync())
                   .Returns(Task.FromResult<Result<AccountResponse>>(
                       Error.NotFound("Test.Error", "Delete account failed.")));

        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();
        ctx.Services.AddSingleton<IAuthenticationService>(authService.Object);

        // act
        var comp = ctx.RenderComponent<PersonalData>();
        comp.Find("#delete-button").Click();

        // assert
        var expectedHtml =
@"
    <h4>Personal data</h4>
<div class=""row"">
  <div class=""col-md-6"">
    <p>
      Your account contains personal data that you have given us.
      This page allows you to download or delete that data.
    </p>
    <p>
      <strong>Deleting this data will permanently remove your account,
              and it cannot be recovered.</strong>
    </p>
    <div class=""row"">
      <div class=""col"">
        <button id=""download-button"" class=""btn btn-primary"" >
          Download
        </button>
      </div>
      <div class=""col"">
        <button id=""delete-button"" class=""btn btn-danger"" >
          Delete data and close account
        </button>
      </div>
    </div>
  </div>
  <div class=""col-12 mt-4"">
    <textarea class=""col-12 col-md-6"" style=""height: 60vh"" readonly="""">
        Error (Test.Error [3]): Delete account failed.
    </textarea>
  </div>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
