//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Api.UnitTests;

[TestClass]
public class RequestTests
{
    [TestMethod]
    public void ChangePasswordRequest_Setters()
    {
        // arrange
        var currentPassword = "password-1";
        var newPassword = "password-2";

        var request = new ChangePasswordRequest("", "");

        // act
        request = request with
        {
            CurrentPassword = currentPassword,
            NewPassword = newPassword
        };

        // assert
        request.Should().NotBeNull();
        request.CurrentPassword.Should().Be(currentPassword);
        request.NewPassword.Should().Be(newPassword);
    }

    [TestMethod]
    public void ChangeRoleRequest_Setters()
    {
        // arrange
        var userName = "TestUser";
        var role = UserRoles.Admin;

        var request = new ChangeRoleRequest("", "");

        // act
        request = request with
        {
            UserName = userName,
            NewRole = role
        };

        // assert
        request.Should().NotBeNull();
        request.UserName.Should().Be(userName);
        request.NewRole.Should().Be(role);
    }

    [TestMethod]
    public void GetResetTokenRequest_Setters()
    {
        // arrange
        var email = "tester@test.com";

        var request = new GetResetTokenRequest("");

        // act
        request = request with
        {
            Email = email,
        };

        // assert
        request.Should().NotBeNull();
        request.Email.Should().Be(email);
    }

    [TestMethod]
    public void LoginRequest_Setters()
    {
        // arrange
        var user = "TestUser";
        var password = "password-42";

        var request = new LoginRequest("", "");

        // act
        request = request with
        {
            UserName = user,
            Password = password
        };

        // assert
        request.Should().NotBeNull();
        request.UserName.Should().Be(user);
        request.Password.Should().Be(password);
    }

    [TestMethod]
    public void RegisterRequest_Setters()
    {
        // arrange
        var userName = "TestUser";
        var given = "Tester";
        var family = "McTest";
        var email = "tester@test.com";
        var password = "password-42";
        var phone = "555";

        var request = new RegisterRequest("", "", "", "", "", null);

        // act
        request = request with
        {
            UserName = userName,
            GivenName = given,
            FamilyName = family,
            Email = email,
            Password = password,
            PhoneNumber = phone
        };

        // assert
        request.Should().NotBeNull();
        request.UserName.Should().Be(userName);
        request.GivenName.Should().Be(given);
        request.FamilyName.Should().Be(family);
        request.Email.Should().Be(email);
        request.Password.Should().Be(password);
        request.PhoneNumber.Should().Be(phone);
    }

    [TestMethod]
    public void UpdateAccountRequest_Setters()
    {
        // arrange
        var userName = "TestUser";
        var given = "Tester";
        var family = "McTest";
        var email = "tester@test.com";
        var phone = "555";

        var request = new UpdateAccountRequest("", "", "", "", null);

        // act
        request = request with
        {
            UserName = userName,
            GivenName = given,
            FamilyName = family,
            Email = email,
            PhoneNumber = phone
        };

        // assert
        request.Should().NotBeNull();
        request.UserName.Should().Be(userName);
        request.GivenName.Should().Be(given);
        request.FamilyName.Should().Be(family);
        request.Email.Should().Be(email);
        request.PhoneNumber.Should().Be(phone);
    }

    [TestMethod]
    public void ResetPasswordRequest_Setters()
    {
        // arrange
        var email = "tester@test.com";
        var token = "test-email-reset-token";
        var newPassword = "password-2";

        var request = new ResetPasswordRequest("", "", "");

        // act
        request = request with
        {
            Email = email,
            ResetToken = token,
            NewPassword = newPassword
        };

        // assert
        request.Should().NotBeNull();
        request.Email.Should().Be(email);
        request.ResetToken.Should().Be(token);
        request.NewPassword.Should().Be(newPassword);
    }
}
