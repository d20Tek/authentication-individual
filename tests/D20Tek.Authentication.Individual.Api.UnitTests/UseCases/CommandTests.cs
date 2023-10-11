//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.ChangePassword;
using D20Tek.Authentication.Individual.UseCases.ChangeRole;

namespace D20Tek.Authentication.Individual.Api.UnitTests.UseCases;

[TestClass]
public class CommandTests
{
    private Guid _testUserId = Guid.NewGuid();

    [TestMethod]
    public void ChangePasswordCommand_Setters()
    {
        // arrange
        var currentPassword = "password-1";
        var newPassword = "password-2";

        var command = new ChangePasswordCommand(new Guid(), "", "");

        // act
        command = command with
        {
            UserId = _testUserId,
            CurrentPassword = currentPassword,
            NewPassword = newPassword
        };

        // assert
        command.Should().NotBeNull();
        command.UserId.Should().Be(_testUserId);
        command.CurrentPassword.Should().Be(currentPassword);
        command.NewPassword.Should().Be(newPassword);
    }

    [TestMethod]
    public void ChangeRoleCommand_Setters()
    {
        // arrange
        var userName = "TestUser";
        var role = UserRoles.Admin;

        var command = new ChangeRoleCommand("", "");

        // act
        command = command with
        {
            UserName = userName,
            NewRole = role
        };

        // assert
        command.Should().NotBeNull();
        command.UserName.Should().Be(userName);
        command.NewRole.Should().Be(role);
    }
}
