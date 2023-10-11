//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.ChangePassword;
using D20Tek.Authentication.Individual.UseCases.ChangeRole;
using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Authentication.Individual.UseCases.RemoveAccount;
using D20Tek.Authentication.Individual.UseCases.ResetPassword;
using D20Tek.Authentication.Individual.UseCases.UpdateAccount;

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

    [TestMethod]
    public void RegisterCommand_Setters()
    {
        // arrange
        var userName = "TestUser";
        var given = "Tester";
        var family = "McTest";
        var email = "tester@test.com";
        var password = "password-42";
        var phone = "555";

        var command = new RegisterCommand("", "", "", "", "", null);

        // act
        command = command with
        {
            UserName = userName,
            GivenName = given,
            FamilyName = family,
            Email = email,
            Password = password,
            PhoneNumber = phone
        };

        // assert
        command.Should().NotBeNull();
        command.UserName.Should().Be(userName);
        command.GivenName.Should().Be(given);
        command.FamilyName.Should().Be(family);
        command.Email.Should().Be(email);
        command.Password.Should().Be(password);
        command.PhoneNumber.Should().Be(phone);
    }

    [TestMethod]
    public void RemoveCommand_Setters()
    {
        // arrange
        var command = new RemoveCommand(new Guid());

        // act
        command = command with
        {
            UserId = _testUserId
        };

        // assert
        command.Should().NotBeNull();
        command.UserId.Should().Be(_testUserId);
    }

    [TestMethod]
    public void ResetPasswordCommand_Setters()
    {
        // arrange
        var email = "tester@test.com";
        var token = "test-email-reset-token";
        var newPassword = "password-2";

        var command = new ResetPasswordCommand("", "", "");

        // act
        command = command with
        {
            Email = email,
            ResetToken = token,
            NewPassword = newPassword
        };

        // assert
        command.Should().NotBeNull();
        command.Email.Should().Be(email);
        command.ResetToken.Should().Be(token);
        command.NewPassword.Should().Be(newPassword);
    }

    [TestMethod]
    public void UpdateAccountCommand_Setters()
    {
        // arrange
        var userName = "TestUser";
        var given = "Tester";
        var family = "McTest";
        var email = "tester@test.com";
        var phone = "555";

        var command = new UpdateCommand(new Guid(), "", "", "", "", null);

        // act
        command = command with
        {
            UserId = _testUserId,
            UserName = userName,
            GivenName = given,
            FamilyName = family,
            Email = email,
            PhoneNumber = phone
        };

        // assert
        command.Should().NotBeNull();
        command.UserId.Should().Be(_testUserId);
        command.UserName.Should().Be(userName);
        command.GivenName.Should().Be(given);
        command.FamilyName.Should().Be(family);
        command.Email.Should().Be(email);
        command.PhoneNumber.Should().Be(phone);
    }
}
