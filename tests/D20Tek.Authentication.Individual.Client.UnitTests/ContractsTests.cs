//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Contracts;

namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public class ContractsTests
{
    [TestMethod]
    public void AccountResponse_Setters()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var userName = "TestUser";
        var given = "Tester";
        var family = "McTest";
        var email = "tester@test.com";
        var phone = "555";

        var response = new AccountResponse("", null, null, null, null, null);

        // act
        response = response with
        {
            UserId = id,
            UserName = userName,
            GivenName = given,
            FamilyName = family,
            Email = email,
            PhoneNumber = phone,
        };

        // assert
        response.Should().NotBeNull();
        response.UserId.Should().Be(id);
        response.UserName.Should().Be(userName);
        response.GivenName.Should().Be(given);
        response.FamilyName.Should().Be(family);
        response.Email.Should().Be(email);
        response.PhoneNumber.Should().Be(phone);
    }

    [TestMethod]
    public void AuthenticationResponse_Setters()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var userName = "TestUser";
        var token = "test-access-token";
        var expiration = DateTime.UtcNow.AddHours(1);
        var refresh = "test-refresh-token";

        var response = new AuthenticationResponse("", "", "", DateTime.UtcNow, "");

        // act
        response = response with
        {
            UserId = id,
            UserName = userName,
            Token = token,
            Expiration = expiration,
            RefreshToken = refresh
        };

        // assert
        response.Should().NotBeNull();
        response.UserId.Should().Be(id);
        response.UserName.Should().Be(userName);
        response.Token.Should().Be(token);
        response.Expiration.Should().Be(expiration);
        response.RefreshToken.Should().Be(refresh);
    }

    [TestMethod]
    public void ResetTokenResponse_Setters()
    {
        // arrange
        var token = "test-email-reset-token";

        var response = new ResetResponse("");

        // act
        response = response with
        {
            ResetToken = token,
        };

        // assert
        response.Should().NotBeNull();
        response.ResetToken.Should().Be(token);
    }

    [TestMethod]
    public void ResetPasswordRequest_Setters()
    {
        // arrange
        var token = "test-email-reset-token";

        var request = new ResetPasswordRequest();

        // act
        request.ResetToken = token;

        // assert
        request.Should().NotBeNull();
        request.ResetToken.Should().Be(token);
    }
}
