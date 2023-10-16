//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Components;

namespace D20Tek.Authentication.Individual.Client.UnitTests.Components;

[TestClass]
public class ProfileLayoutTests
{
    [TestMethod]
    public void Render_WithAuthenticatedUser_ShowsLayout()
    {
        // arrange
        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();

        // act
        var comp = ctx.RenderComponent<ProfileLayout>();

        // assert
        var expectedHtml =
@"
<div class=""page"" >
  <div class=""sidebar"" >
    <div class=""top-row ps-3 navbar navbar-dark"" >
      <div class=""container-fluid"" >
        <a class=""navbar-brand"" href="""" >Authentication</a>
        <button title=""Navigation menu"" class=""navbar-toggler""  >
          <span class=""navbar-toggler-icon"" ></span>
        </button>
      </div>
    </div>
    <div class=""collapse nav-scrollable""  >
      <nav class=""flex-column"" >
        <div class=""nav-item px-3"" >
          <a href="""" class=""nav-link active"">
            <span class=""oi oi-home"" aria-hidden=""true"" ></span>
            Home
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/profile"" class=""nav-link"">
            <span class=""oi oi-person"" aria-hidden=""true"" ></span>
            Profile
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/change-password"" class=""nav-link"">
            <span class=""oi oi-key"" aria-hidden=""true"" ></span>
            Change Password
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/personal-data"" class=""nav-link"">
            <span class=""oi oi-cloud-download"" aria-hidden=""true"" ></span>
            Personal data
          </a>
        </div>
      </nav>
    </div>
  </div>
  <main >
    <div class=""top-row px-4"" >
      <a class=""mx-2"" href=""/authentication/profile"">
        Hello, TestUser!
      </a>|
      <a class=""mx-2"" href=""/authentication/logout"">Log out</a>
    </div>
    <article class=""content px-4"" >
      <h2 class=""mb-3"" >Manage your account</h2>
      <h3 class=""mb-2"" >Change your account settings</h3>
      <hr >
    </article>
  </main>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithUnauthenticatedUser_ShowsLayout()
    {
        // arrange
        using var ctx = new TestContext();
        ctx.AddTestAuthorization();

        // act
        var comp = ctx.RenderComponent<ProfileLayout>();

        // assert
        var expectedHtml =
@"
<div class=""page"" >
  <div class=""sidebar"" >
    <div class=""top-row ps-3 navbar navbar-dark"" >
      <div class=""container-fluid"" >
        <a class=""navbar-brand"" href="""" >Authentication</a>
        <button title=""Navigation menu"" class=""navbar-toggler""  >
          <span class=""navbar-toggler-icon"" ></span>
        </button>
      </div>
    </div>
    <div class=""collapse nav-scrollable""  >
      <nav class=""flex-column"" >
        <div class=""nav-item px-3"" >
          <a href="""" class=""nav-link active"">
            <span class=""oi oi-home"" aria-hidden=""true"" ></span>
            Home
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/profile"" class=""nav-link"">
            <span class=""oi oi-person"" aria-hidden=""true"" ></span>
            Profile
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/change-password"" class=""nav-link"">
            <span class=""oi oi-key"" aria-hidden=""true"" ></span>
            Change Password
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/forgot-password"" class=""nav-link"">
            <span class=""oi oi-lock-locked"" aria-hidden=""true"" ></span>
            Forgot Password
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/personal-data"" class=""nav-link"">
            <span class=""oi oi-cloud-download"" aria-hidden=""true"" ></span>
            Personal data
          </a>
        </div>
      </nav>
    </div>
  </div>
  <main >
    <div class=""top-row px-4"" >
      <a class=""mx-2"" href=""/authentication/register"">Register</a>|
      <a class=""mx-2"" href=""/authentication/login"">Log in</a>
    </div>
    <article class=""content px-4"" >
      <h2 class=""mb-3"" >Manage your account</h2>
      <h3 class=""mb-2"" >Change your account settings</h3>
      <hr >
    </article>
  </main>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void ToggleNavDisplay()
    {
        // arrange
        using var ctx = new TestContext();
        ctx.AddSimpleAppAuthorization();

        // act
        var comp = ctx.RenderComponent<ProfileLayout>();
        comp.Find(".navbar-toggler").Click();

        // assert
        var expectedHtml =
@"
<div class=""page"" >
  <div class=""sidebar"" >
    <div class=""top-row ps-3 navbar navbar-dark"" >
      <div class=""container-fluid"" >
        <a class=""navbar-brand"" href="""" >Authentication</a>
        <button title=""Navigation menu"" class=""navbar-toggler""  >
          <span class=""navbar-toggler-icon"" ></span>
        </button>
      </div>
    </div>
    <div class=""nav-scrollable""  >
      <nav class=""flex-column"" >
        <div class=""nav-item px-3"" >
          <a href="""" class=""nav-link active"">
            <span class=""oi oi-home"" aria-hidden=""true"" ></span>
            Home
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/profile"" class=""nav-link"">
            <span class=""oi oi-person"" aria-hidden=""true"" ></span>
            Profile
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/change-password"" class=""nav-link"">
            <span class=""oi oi-key"" aria-hidden=""true"" ></span>
            Change Password
          </a>
        </div>
        <div class=""nav-item px-3"" >
          <a href=""/authentication/personal-data"" class=""nav-link"">
            <span class=""oi oi-cloud-download"" aria-hidden=""true"" ></span>
            Personal data
          </a>
        </div>
      </nav>
    </div>
  </div>
  <main >
    <div class=""top-row px-4"" >
      <a class=""mx-2"" href=""/authentication/profile"">
        Hello, TestUser!
      </a>|
      <a class=""mx-2"" href=""/authentication/logout"">Log out</a>
    </div>
    <article class=""content px-4"" >
      <h2 class=""mb-3"" >Manage your account</h2>
      <h3 class=""mb-2"" >Change your account settings</h3>
      <hr >
    </article>
  </main>
</div>
";

        comp.MarkupMatches(expectedHtml);
    }
}
