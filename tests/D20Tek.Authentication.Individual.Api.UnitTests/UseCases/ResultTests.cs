//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.UseCases;
using D20Tek.Authentication.Individual.UseCases.GetResetToken;

namespace D20Tek.Authentication.Individual.Api.UnitTests.UseCases;

[TestClass]
public class ResultTests
{
    private Guid _testUserId = Guid.NewGuid();

    [TestMethod]
    public void AccountResult_Setters()
    {
        // arrange
        var userName = "TestUser";
        var given = "Tester";
        var family = "McTest";
        var email = "tester@test.com";
        var phone = "555";

        var result = new AccountResult(new Guid(), null, null, null, null, null);

        // act
        result = result with
        {
            UserId = _testUserId,
            UserName = userName,
            GivenName = given,
            FamilyName = family,
            Email = email,
            PhoneNumber = phone,
        };

        // assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(_testUserId);
        result.UserName.Should().Be(userName);
        result.GivenName.Should().Be(given);
        result.FamilyName.Should().Be(family);
        result.Email.Should().Be(email);
        result.PhoneNumber.Should().Be(phone);
    }

    [TestMethod]
    public void AuthenticationResult_Setters()
    {
        // arrange
        var userName = "TestUser";
        var token = "test-access-token";
        var expiration = DateTime.UtcNow.AddHours(1);
        var refresh = "test-refresh-token";

        var result = new AuthenticationResult("", "", "", DateTime.UtcNow, "");

        // act
        result = result with
        {
            UserId = _testUserId.ToString(),
            UserName = userName,
            Token = token,
            Expiration = expiration,
            RefreshToken = refresh
        };

        // assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(_testUserId.ToString());
        result.UserName.Should().Be(userName);
        result.Token.Should().Be(token);
        result.Expiration.Should().Be(expiration);
        result.RefreshToken.Should().Be(refresh);
    }

    [TestMethod]
    public void ResetTokenResult_Setters()
    {
        // arrange
        var resetCode = "test-reset-token";

        var result = new ResetTokenResult("");

        // act
        result = result with
        {
            ResetCode = resetCode,
        };

        // assert
        result.Should().NotBeNull();
        result.ResetCode.Should().Be(resetCode);
    }

    [TestMethod]
    public void TokenResponse_Setters()
    {
        // arrange
        var token = "test-token";
        var validTo = DateTime.UtcNow.AddHours(1);

        var response = new TokenResponse("", DateTime.UtcNow);

        // act
        response = response with
        {
            Token = token,
            ValidTo = validTo
        };

        // assert
        response.Should().NotBeNull();
        response.Token.Should().Be(token);
        response.ValidTo.Should().Be(validTo);
    }
}
