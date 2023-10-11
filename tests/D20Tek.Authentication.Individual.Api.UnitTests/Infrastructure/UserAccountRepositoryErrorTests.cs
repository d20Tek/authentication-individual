//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Authentication.Individual.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Infrastructure;

[TestClass]
public class UserAccountRepositoryErrorTests
{
    private readonly ILogger<UserAccountRepository> _logger =
        new Mock<ILogger<UserAccountRepository>>().Object;

    [TestMethod]
    public async Task AttachUserRoleAsync_CreateRoleFails_ReturnsFalse()
    {
        // arrange
        var userMgr = new TestUserManager();
        var roleMgr = CreateRoleManager();

        var account = AccountCommandFactory.CreateAccount("TestAccount");
        var repo = new UserAccountRepository(userMgr, roleMgr, _logger);

        // act
        var result = await repo.AttachUserRoleAsync(account, "testRole");

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task AttachUserRoleAsync_AddToRoleFails_ReturnsFalse()
    {
        // arrange
        var userMgr = new TestUserManager();
        var roleMgr = CreateRoleManager(true);

        var account = AccountCommandFactory.CreateAccount("TestAccount");
        var repo = new UserAccountRepository(userMgr, roleMgr, _logger);

        // act
        var result = await repo.AttachUserRoleAsync(account, "testRole");

        // assert
        result.Should().BeFalse();
    }

    private RoleManager<IdentityRole> CreateRoleManager(bool roleExists = false)
    {
        var store = new Mock<IRoleStore<IdentityRole>>();
        store.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(IdentityResult.Failed());

        if (roleExists)
        {
            store.Setup(x => x.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new IdentityRole("testRole"));
        }

        var logger = new Mock<ILogger<RoleManager<IdentityRole>>>().Object;

        return new RoleManager<IdentityRole>(
            store.Object,
            Array.Empty<IRoleValidator<IdentityRole>>(),
            null,
            null,
            logger);
    }
}

internal class TestUserManager : UserManager<UserAccount>
{
    public TestUserManager()
        : base(
            new Mock<IUserStore<UserAccount>>().Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null)
    {
    }

    public override Task<IdentityResult> AddToRoleAsync(UserAccount user, string role)
    {
        return Task.FromResult(IdentityResult.Failed());
    }
}
